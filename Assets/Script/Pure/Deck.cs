using System;
using System.Collections.Generic;
using Raziel.Battle;

namespace Raziel.Pure.Deck
{
    public class Deck
    {
        private readonly List<Card> _decks = new();
        public IReadOnlyList<Card> Decks => _decks;

        private readonly List<CardInstance> _battleDeck = new();
        public List<CardInstance> BattleDeck => _battleDeck;

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

        public CardInstance DrawCard()
        {
            if (_battleDeck.Count == 0) return null;

            int index = UnityEngine.Random.Range(0, _battleDeck.Count);

            var card = _battleDeck[index];

            _battleDeck.RemoveAt(index);

            return card;
        }

    }
}