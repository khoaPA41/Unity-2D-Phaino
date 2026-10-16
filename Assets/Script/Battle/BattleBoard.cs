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

        // public event Action<BoardSlot, int> SummonAction = delegate { };
        public event Action<Character, CardInstance, int> SummonAction = delegate { };

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
            Debug.Log("Summon");
            if (cardInstance == null) return false;

            var board = GetSlotByType(character, cardInstance.CardReference.CardType);

            if (board == null || index < 0 || index >= board.Length) return false;
            Debug.Log("Have board");
            if (index < 0 || index >= board.Length) return false;
            Debug.Log("nice index");

            if (board[index].CardInstance.CardReference != null) return false;
            Debug.Log("Have slot");

            board[index].CardInstance = cardInstance;
            // board[index].IsAttackStance = isAttackStance;

            SummonAction?.Invoke(character, cardInstance, index);
            return true;
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
    }
}
