using System;
using UnityEngine;

namespace Raziel.Battle
{
    public class BattleBoard : MonoBehaviour
    {
        public static BattleBoard Instance;

        // Player Field
        public BoardSlot[] PlayerSoulField = new BoardSlot[5];
        public BoardSlot[] PlayerFunctionalField = new BoardSlot[5];

        // Enemy Field
        public BoardSlot[] EnemySouldField = new BoardSlot[5];
        public BoardSlot[] EnemyFunctionalField = new BoardSlot[5];

        public event Action<Character, CardInstance, int, BoardSlot> SummonAction = delegate { };
        public event Action<BoardSlot> CardOnBoardClicked;
        public event Action<Character, BoardSlot> ReturnAttackStance;
        public event Action<Character> EndGame = delegate { };
        public event Action<BoardSlot> SlotChanged = delegate { };

        private BoardSlot _attackerSlot;
        private BoardSlot _targetSlot;

        [SerializeField] private DuelInformation _player;
        [SerializeField] private DuelInformation _enemy;

        private BattleResolver _battleResolver;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _battleResolver = new();
        }

        private void Start()
        {
            SetupSoulField(PlayerSoulField, Character.Player);
            SetupSoulField(EnemySouldField, Character.Enemy);


            SetupFunctionalField(PlayerFunctionalField, Character.Player);
            SetupFunctionalField(EnemyFunctionalField, Character.Enemy);
        }

        private void SetupSoulField(BoardSlot[] slots, Character character)
        {
            foreach (var slot in slots)
            {
                slot.Character = character;
                slot.AcceptSoul = true;
            }
        }

        private void SetupFunctionalField(BoardSlot[] slots, Character character)
        {
            foreach (var slot in slots)
            {
                slot.Character = character;
                slot.AcceptSoul = false;
            }
        }

        public bool SummonCard(CardInstance cardInstance, bool isAttackStance, Character character, int index)
        {
            if (!CanSummonCard(cardInstance, character, index)) return false;

            var board = GetSlotByType(character, cardInstance.CardReference.CardType);

            board[index].CardInstance = cardInstance;


            board[index].CardInstance.HasUsedDefense = !isAttackStance;
            board[index].CardInstance.IsAttackStance = isAttackStance;

            SummonAction?.Invoke(character, cardInstance, index, board[index]);

            return true;
        }

        public bool CanSummonCard(CardInstance cardInstance, Character character, int index)
        {
            if (cardInstance == null || cardInstance.CardReference == null) return false;

            var board = GetSlotByType(character, cardInstance.CardReference.CardType);

            if (board == null || index < 0 || index >= board.Length) return false;

            var slot = board[index];
            if (slot == null) return false;

            return slot.CardInstance == null || slot.CardInstance.CardReference == null;
        }

        public BoardSlot[] GetSlotByType(Character character, CardType cardType)
        {
            if (character == Character.Player)
            {
                if (cardType == CardType.Soul)
                {
                    return PlayerSoulField;
                }
                return PlayerFunctionalField;
            }

            if (cardType == CardType.Soul)
            {
                return EnemySouldField;
            }

            return EnemyFunctionalField;
        }

        public bool HasSoulCard(Character character, CardType cardType)
        {
            var slots = GetSlotByType(character, cardType);

            if (slots == null) return false;

            foreach (var soul in slots)
            {
                if (soul?.CardInstance?.CardReference != null) return true;
            }
            return false;
        }

        // Board
        public void OnCardCliked(BoardSlot boardSlot)
        {
            CardOnBoardClicked?.Invoke(boardSlot);
        }

        public void NotiySlotChanged(BoardSlot boardSlot) => SlotChanged?.Invoke(boardSlot);

        public void SetAttacker(BoardSlot attacker)
        {
            _attackerSlot = attacker;

            if (_attackerSlot?.CardInstance?.CardReference == null || !_attackerSlot.CardInstance.IsAttackStance) return;

            Debug.Log($"Attacker: {attacker.CardInstance.CardReference.CardNameId}");
        }

        public void SetTarget(BoardSlot target)
        {
            _targetSlot = target;

            if (target?.CardInstance?.CardReference == null) return;

            Debug.Log($"Targeter: {target.CardInstance.CardReference.CardNameId}");
        }

        public void Attack()
        {
            if (_attackerSlot == null || _attackerSlot.CardInstance == null || _attackerSlot.CardInstance.CardReference == null) return;

            DuelInformation targetDirect;
            if (CanDirectAttack(_attackerSlot))
            {
                targetDirect = _attackerSlot.Character == Character.Player ? _enemy : _player;
                targetDirect.SubtractFragmentSoul();
                if (targetDirect.FragmentSoul <= 0) EndGame?.Invoke(targetDirect.character);
                return;
            }

            if (_targetSlot == null || _targetSlot.CardInstance == null || _targetSlot.CardInstance.CardReference == null) return;

            var uiManager = UiBattleManagers.Instance;
            if (uiManager == null) return;

            var attacker = _attackerSlot.CardInstance;

            if (!attacker.IsAttackStance) return;

            var targeter = _targetSlot.CardInstance;

            if (attacker.CardReference.CardType != CardType.Soul || targeter.CardReference.CardType != CardType.Soul) return;

            var result = _battleResolver.Attack(attacker, targeter);

            if (result.AttackerDies)
            {
                Debug.Log("Attacker death: " + attacker.CardReference.CardNameId + " - " + _attackerSlot.Character);
                CardDeath(_attackerSlot);
            }

            if (result.TargeterDies)
            {
                Debug.Log("Targeter death: " + targeter.CardReference.CardNameId + " - " + _targetSlot.Character);
                CardDeath(_targetSlot);
            }

            if (!result.AttackerDies)
            {
                Debug.Log("Attacker: " + attacker.CardReference.CardNameId + ": " + _attackerSlot.CardInstance.BattleStar);
                uiManager.OnUpdateStarOnBoard(_attackerSlot, result.AttackerStarAfter);
            }

            if (!result.TargeterDies)
            {
                Debug.Log("Targeter: " + targeter.CardReference.CardNameId + ": " + _targetSlot.CardInstance.BattleStar);
                uiManager.OnUpdateStarOnBoard(_targetSlot, result.TargeterStarAfter);
            }

            _attackerSlot = null;
            _targetSlot = null;
        }

        private bool CanDirectAttack(BoardSlot attackerSlot)
        {
            if (!attackerSlot.CardInstance.IsAttackStance) return false;

            var targetSoulField = attackerSlot.Character == Character.Player ? EnemySouldField : PlayerSoulField;

            foreach (var slot in targetSoulField)
            {
                if (slot?.CardInstance?.CardReference != null) return false;
            }
            return true;
        }

        public void CardDeath(BoardSlot boardSlot)
        {
            var uiManager = UiBattleManagers.Instance;
            if (uiManager == null) return;

            if (boardSlot.Character == Character.Player)
            {
                _player.OnCardDeath(boardSlot.CardInstance);
            }
            else
            {
                _enemy.OnCardDeath(boardSlot.CardInstance);
            }

            uiManager.ReleaseCardOnBoardByCharacterAndIndex(boardSlot.Character, boardSlot);
            boardSlot.CardInstance = null;
        }

        public BoardSlot FindSlot(CardInstance cardInstance)
        {
            foreach (var field in new[] { PlayerFunctionalField, EnemyFunctionalField })
            {
                foreach (var slot in field)
                {
                    if (slot.CardInstance == cardInstance) return slot;
                }
            }
            return null;
        }

        public void ProcessDefenseElapsed(Character character)
        {
            var boardSlots = character == Character.Player ? PlayerSoulField : EnemySouldField;

            foreach (var stanceSlot in boardSlots)
            {
                var card = stanceSlot?.CardInstance;
                if (card == null || card.CardReference == null) continue;

                if (card.IsAttackStance) continue;

                card.DefTurnsElapsed++;

                if (card.DefTurnsElapsed > card.DefTurnLimit)
                {
                    card.IsAttackStance = true;
                    ReturnAttackStance?.Invoke(character, stanceSlot);
                }
            }
        }

        public void ProcessEffectDuration(Character ownerTurn)
        {
            TickField(PlayerSoulField, ownerTurn);
            TickField(EnemySouldField, ownerTurn);
        }

        private void TickField(BoardSlot[] boardSlots, Character ownerTurn)
        {
            foreach (var slot in boardSlots)
            {
                var card = slot?.CardInstance;
                if (card?.CardReference == null) continue;
                if (card.TickModifiers(ownerTurn)) SlotChanged?.Invoke(slot);
                card.RemoveStarFrom(card);
            }
        }
    }
}
