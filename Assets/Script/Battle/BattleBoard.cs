using System;
using System.Collections;
using System.Linq;
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
        public event Action<CardInstance> CardLeaveBoard = delegate { };

        public event Action OnAttack = delegate { };

        // Rupture
        public event Action UseRupture = delegate { };
        public event Action ChooseRupture = delegate { };

        public BoardSlot _attackerSlot { get; private set; }
        public BoardSlot _targetSlot { get; private set; }

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

        private void OnEnable()
        {
            OnAttack += Attack;
        }

        private void OnDisable()
        {
            OnAttack -= Attack;
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
                if (targetDirect.FragmentSoul <= 0)
                {
                    EndGame?.Invoke(targetDirect.character);
                    return;
                }
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

        public bool CanAttack(Character turnOwner, BoardSlot attacker, BoardSlot targeter)
        {
            if (attacker?.CardInstance?.CardReference == null) return false;
            if (attacker.Character != turnOwner) return false;
            if (attacker?.CardInstance?.IsAttacked == true) return false;

            var soulSlots = GetSlotByType(turnOwner, CardType.Soul);
            if (soulSlots == null || !soulSlots.Contains(attacker)) return false;

            if (attacker.CardInstance.CardReference.CardType != CardType.Soul) return false;
            if (!attacker.CardInstance.IsAttackStance || attacker.CardInstance.BattleStar <= 0) return false;


            var defendingSide = attacker.Character == Character.Player ? Character.Enemy : Character.Player;

            if (targeter == null)
                return !HasSoulCard(defendingSide, CardType.Soul);

            if (targeter?.CardInstance?.CardReference == null) return false;
            if (targeter.Character != defendingSide) return false;


            var defendingSidesoulSlots = GetSlotByType(defendingSide, CardType.Soul);

            if (defendingSidesoulSlots == null || !defendingSidesoulSlots.Contains(targeter)) return false;

            if (targeter?.CardInstance?.CardReference.CardType != CardType.Soul) return false;
            return targeter.CardInstance.BattleStar > 0;
        }

        public bool HasRuptureCard()
        {
            Debug.Log("Check Rupture Card if has");

            if (_targetSlot == null) // Neu khong co muc tieu / tam thoi tan cong truc tiep
            {
                return false;
            }

            var ruptureOfEnemy = GetSlotByType(_targetSlot.Character, CardType.Rupture);
            var hasAtleastOneRupture = false;

            foreach (var ruptureCard in ruptureOfEnemy) // Check rupture card
            {
                if (ruptureCard?.CardInstance?.CardReference?.Effect == null) continue;
                if (ruptureCard?.CardInstance.HasUsedEffect == true) continue;
                if (ruptureCard.CardInstance.CardReference.CardType != CardType.Rupture) continue;
                hasAtleastOneRupture = true;
            }

            bool canAttackImmediately = !hasAtleastOneRupture;

            if (!canAttackImmediately) // if has rupture on enemy side
            {
                return true;
            }

            return false;
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
            var card = boardSlot?.CardInstance;
            if (card?.CardReference == null) return;

            boardSlot.CardInstance = null;

            CardLeaveBoard?.Invoke(card);

            if (boardSlot.Character == Character.Player)
            {
                _player.OnCardDeath(card);
            }
            else
            {
                _enemy.OnCardDeath(card);
            }

            var uiManager = UiBattleManagers.Instance;
            if (uiManager == null) return;
            uiManager.ReleaseCardOnBoardByCharacterAndIndex(boardSlot.Character, boardSlot);
            // Nên dùng Event để giảm phụ thuộc
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

        public void ResetAttack()
        {
            foreach (var field in new[] { PlayerSoulField, EnemySouldField })
            {
                foreach (var slot in field)
                {
                    if (slot?.CardInstance?.CardReference == null) continue;
                    slot.CardInstance.IsAttacked = false;
                }
            }
        }
    }
}
