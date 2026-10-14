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
    }

    public class DuelInformation : MonoBehaviour
    {
        [SerializeField] private List<Card> _cards;
        public Character character;
        public Deck _deck;

        public List<CardInstance> CardsHand;

        public event Action DrawCardSucces = delegate { };

        public void SetupDeck()
        {
            _deck = new();
            foreach (var card in _cards)
            {
                _deck.AddCard(card);
            }
            _deck.InitialDeck();
        }

        public void DrawCard()
        {
            CardsHand.Add(_deck.DrawCard());
            DrawCardSucces?.Invoke();
        }
    }
}