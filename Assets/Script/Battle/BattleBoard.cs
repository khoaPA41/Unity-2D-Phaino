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
            if (!CanSummonCard(cardInstance, character, index)) return false;

            var board = GetSlotByType(character, cardInstance.CardReference.CardType);

            board[index].CardInstance = cardInstance;

            board[index].CardInstance.IsAttackStance = isAttackStance;

            SummonAction?.Invoke(character, cardInstance, index);
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
    }
}
