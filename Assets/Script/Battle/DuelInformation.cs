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
        public bool IsAttackStance; // Card State
    }

    public class DuelInformation : MonoBehaviour
    {
        [SerializeField] private List<Card> _cards;
        public Character character;
        public Deck Deck { get; private set; }

        public event Action<CardInstance> UpdateCardHandAction = delegate { };

        public CardInstance SelectedCard = null;

        private void OnEnable()
        {
            Deck = new();
            Deck.ChangesInHand += UpdateCardHand;
        }

        private void OnDisable()
        {
            Deck.ChangesInHand -= UpdateCardHand;
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
            Deck.DrawCard(5);
        }

        public void DrawCard()
        {
            Deck.DrawCard(1);
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
            Deck.OnChangeInHand();
        }
    }
}