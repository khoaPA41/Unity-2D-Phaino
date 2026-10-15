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

        public event Action DrawCardSucces = delegate { };

        public CardInstance SelectedCard;

        public int index = 0;


        private void OnEnable()
        {
            DrawCardSucces += UpdateCardHand;
        }

        private void OnDisable()
        {
            DrawCardSucces -= UpdateCardHand;
        }

        public void SetupDeck()
        {
            Deck = new();
            foreach (var card in _cards)
            {
                Deck.AddCard(card);
            }
            Deck.InitialDeck();
        }

        public void DrawCardFirstTimes()
        {
            Deck.DrawCard(5);
            DrawCardSucces?.Invoke();
        }

        public void DrawCard()
        {
            Deck.DrawCard(1);
            DrawCardSucces?.Invoke();
        }

        public void UpdateCardHand()
        {
            foreach (var card in Deck.Hand)
            {
                UiBattleManagers.Instance.SpawnCardElementOnHand(character, card);
            }
        }

        private void LogCard(IEnumerable objects)
        {
            foreach (var item in objects)
            {
                if (item is CardInstance cardInstance)
                    Debug.Log(cardInstance.CardReference.CardNameId);
            }
        }

        public void ChooseCard(int index)
        {
            SelectedCard = Deck.Hand[index];
            // Debug.Log(SelectedCard.CardReference.CardNameId);
        }

        public void Summon()
        {
            BattleBoard.Instance.SummonCard(
                  SelectedCard,
                  true,
                  character,
                  index++
              );

            SelectedCard = null;
        }

        // private void UpdateCardHand()
        // {

        // }
    }
}