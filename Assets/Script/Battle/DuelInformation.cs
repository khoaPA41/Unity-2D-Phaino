using System;
using System.Collections;
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

        public int SoulRowIndex = 0;
        public int FunctionalRowIndex = 0;

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

            for(int i = 0; i < Deck.Hand.Count; i++)
            {
                var card = Deck.Hand[i];
                UiBattleManagers.Instance.SpawnCardElementOnHand(character, card, i);
            }
        }

        public void ChooseCard(int index)
        {
            if (index < 0 || index >= Deck.Hand.Count) return;

            SelectedCard = Deck.Hand[index];

            if (SelectedCard == null) return;

            //Debug.Log(SelectedCard);
        }

        public void Summon(int handIndex, int boardIndex)
        {
            Debug.Log(SelectedCard);
            if (SelectedCard == null || SelectedCard.CardReference == null) return;

            if (!BattleBoard.Instance.SummonCard(
                  SelectedCard,
                  true,
                  character,
                  boardIndex
            //SelectedCard.CardReference.CardType == CardType.Soul ? SoulRowIndex++ : FunctionalRowIndex++
            )) return;

            if (!Deck.UseCardOnHand(handIndex)) return;
            SelectedCard = null;
        }
    }
}