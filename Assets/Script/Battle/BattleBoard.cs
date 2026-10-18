using System;
using UnityEngine;

namespace Raziel.Battle
{
    [Serializable]
    public class BoardSlot
    {
        // public int SlotIndex;
        public Character Character;
        public CardInstance CardInstance;
        public bool AcceptSoul; // CardType
    }

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

        public event Action<BoardSlot, int> ChooseCardOnBoard = delegate { };
        public event Action<BoardSlot, int> ChooseEnemyCardOnBoard = delegate { };

        public BoardSlot SelectedSlot;
        public BoardSlot SelectedEnemySlot;

        public int PlayerCardIndex;
        public int EnemtCardIndex;

        [SerializeField] private DuelInformation _player;
        [SerializeField] private DuelInformation _enemy;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
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
            ChooseCardOnBoard += SetBoardSlot;
            ChooseEnemyCardOnBoard += SetEnemyBoardSlot;
            // _player.Deck.CardDeath += 
        }

        private void OnDisable()
        {
            ChooseCardOnBoard -= SetBoardSlot;
            ChooseEnemyCardOnBoard -= SetEnemyBoardSlot;
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

        private BoardSlot[] GetSlotByType(Character character, CardType cardType)
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


        // Board
        public void OnGetBoardSlot(BoardSlot boardSlot, int cardIndex)
        {
            ChooseCardOnBoard?.Invoke(boardSlot, cardIndex);
        }

        public void OnGetEnemyBoardSlot(BoardSlot boardSlot, int cardIndex)
        {
            ChooseEnemyCardOnBoard?.Invoke(boardSlot, cardIndex);
        }

        public void SetBoardSlot(BoardSlot boardSlot, int cardIndex)
        {
            SelectedSlot = boardSlot;
            PlayerCardIndex = cardIndex;
        }

        public void SetEnemyBoardSlot(BoardSlot boardSlot, int cardIndex)
        {
            SelectedEnemySlot = boardSlot;
            EnemtCardIndex = cardIndex;
        }

        public void Attack()
        {
            if (SelectedSlot == null || SelectedSlot.CardInstance == null || SelectedSlot.CardInstance.CardReference == null ||
            SelectedEnemySlot == null || SelectedEnemySlot.CardInstance == null || SelectedEnemySlot.CardInstance.CardReference == null) return;

            var uiManager = UiBattleManagers.Instance;

            if (uiManager == null) return;

            var playerCard = SelectedSlot.CardInstance;
            var enemyCard = SelectedEnemySlot.CardInstance;

            var subtractStar = playerCard.OriginalStar - enemyCard.OriginalStar;

            if (subtractStar == 0)
            {
                Debug.Log("Draw");
                _player.Deck.OnCardDeath(playerCard);
                _enemy.Deck.OnCardDeath(enemyCard);
                uiManager.ReleaseCardOnBoardByCharacterAndIndex(Character.Player, SelectedSlot);
                uiManager.ReleaseCardOnBoardByCharacterAndIndex(Character.Enemy, SelectedEnemySlot);
                ResetBoardSlot(SelectedSlot);
                ResetBoardSlot(SelectedEnemySlot);
            }

            if (subtractStar > 0)
            {
                Debug.Log("Win");
                _enemy.Deck.OnCardDeath(enemyCard);
                uiManager.ReleaseCardOnBoardByCharacterAndIndex(Character.Enemy, SelectedEnemySlot);
                ResetBoardSlot(SelectedEnemySlot);
            }

            if (subtractStar < 0)
            {
                Debug.Log("Defeat");
                _player.Deck.OnCardDeath(playerCard);
                uiManager.ReleaseCardOnBoardByCharacterAndIndex(Character.Player, SelectedSlot);
                ResetBoardSlot(SelectedSlot);
            }

            SelectedSlot = null;
            SelectedEnemySlot = null;


            PlayerCardIndex = -1;
            EnemtCardIndex = -1;


        }

        private void ResetBoardSlot(BoardSlot boardSlot)
        {
            boardSlot.CardInstance = null;
        }
    }
}
