using System;
using System.Collections.Generic;
using Raziel.Pure.Deck;
using UnityEngine;

namespace Raziel.Battle
{
    public enum Character
    {
        Player, Enemy
    }

    [Serializable]
    public class CardInstance
    {
        public Card CardReference;
        public int OriginalStar;
        public int BattleStar;
        public bool IsAttackStance; // Card State
        public int DefTurnsElapsed;
        public int DefTurnLimit;
        public bool HasUsedDefense;
    }

    public class DuelInformation : MonoBehaviour
    {
        [SerializeField] private List<Card> _cards;
        public Character character;
        public Deck Deck { get; private set; }

        public event Action<CardInstance> UpdateCardHandAction = delegate { };
        public event Action ChangesInHand = delegate { };
        public event Action<CardInstance> CardDeath = delegate { };

        public CardInstance SelectedCard = null;

        private void Awake()
        {
            Deck = new();
        }

        private void OnEnable()
        {
            ChangesInHand += UpdateCardHand;
            CardDeath += Deck.AddToGrave;
        }

        private void OnDisable()
        {
            ChangesInHand -= UpdateCardHand;
            CardDeath -= Deck.AddToGrave;
        }

        public void SetupDeck()
        {
            foreach (var card in _cards)
            {
                Deck.AddCard(card);
            }
            Deck.InitialDeck();
        }

        public void DrawCardFirstTimes()
        {
            if (!Deck.DrawCard(5)) return;
            ChangesInHand?.Invoke();
        }

        public void DrawCard()
        {
            if (!Deck.DrawCard(1)) return;
            ChangesInHand?.Invoke();
        }

        public void UpdateCardHand()
        {
            UiBattleManagers.Instance.ReleaseAllCardHand(character);

            for (int i = 0; i < Deck.Hand.Count; i++)
            {
                var card = Deck.Hand[i];
                UiBattleManagers.Instance.SpawnCardElementOnHand(character, card, i);
            }
        }

        public void ChooseCard(int index)
        {
            if (index < 0 || index >= Deck.Hand.Count) return;

            SelectedCard = Deck.Hand[index];
            Debug.Log(SelectedCard.CardReference.CardNameId);
            if (SelectedCard == null) return;
        }

        public void Summon(int handIndex, int boardIndex, bool isAttackStance)
        {
            if (SelectedCard == null || SelectedCard.CardReference == null) return;

            var board = BattleBoard.Instance;
            if (board == null) return;

            if (!board.CanSummonCard(SelectedCard, character, boardIndex)) return;
            if (!Deck.UseCardOnHand(handIndex)) return;

            if (!BattleBoard.Instance.SummonCard(
                SelectedCard,
                isAttackStance,
                character,
                boardIndex
            )) return;

            SelectedCard = null;
            ChangesInHand?.Invoke();
        }

        public void OnCardDeath(CardInstance cardInstance)
        {
            CardDeath?.Invoke(cardInstance);
        }

        // private void Check(IEnumerable objects)
        // {
        //     foreach (var item in objects)
        //     {
        //         var cardInstance = (CardInstance)item;

        //         Debug.Log(cardInstance.CardReference);
        //     }
        // }
    }
}