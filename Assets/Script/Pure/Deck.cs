using System;
using System.Collections.Generic;
using Raziel.Battle;

namespace Raziel.Pure.Deck
{
    public class Deck
    {
        private readonly List<Card> _decks = new(); // Keep reference to Card
        public IReadOnlyList<Card> Decks => _decks;

        private readonly List<CardInstance> _battleDeck = new(); // Deck use to draw
        public List<CardInstance> BattleDeck => _battleDeck;


        private readonly List<CardInstance> _graveDeck = new(); // Deck for card death
        public List<CardInstance> GraveDeck => _graveDeck;


        private readonly List<CardInstance> _hand = new(); // Card on hand
        public List<CardInstance> Hand => _hand;


        private readonly List<CardInstance> _board = new(); // Card on hand
        public List<CardInstance> Board => _board;

        // Event
        public event Action ChangesInHand = delegate { };


        // Setup for battle deck
        public void InitialDeck()
        {
            _battleDeck.Clear();

            foreach (var card in _decks)
            {
                _battleDeck.Add(new CardInstance
                {
                    CardReference = card,
                    OriginalStar = card.CardStar
                });
            }
        }

        public void AddCard(Card card)
        {
            _decks.Add(card);
        }

        // Hand
        public void DrawCard(int cardQuantity)
        {
            if (cardQuantity <= 0 || _battleDeck.Count == 0) return;

            for (int i = 0; i < cardQuantity; i++)
            {
                int index = UnityEngine.Random.Range(0, _battleDeck.Count);

                var card = _battleDeck[index];

                _battleDeck.RemoveAt(index);
                _hand.Add(card);
            }
            ChangesInHand?.Invoke();
        }

        public bool UseCardOnHand(int index)
        {
            if (_hand.Count == 0 || index < 0 || index >= _hand.Count) return false;
            _hand.RemoveAt(index);
            // ChangesInHand?.Invoke();
            return true;
        }

        public CardInstance ChooseCard(CardNameId id)
        {
            return _hand.Find(card => card.CardReference.CardNameId == id);
        }

        public void OnChangeInHand()
        {
            ChangesInHand?.Invoke();
        }
    }
}