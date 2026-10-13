using System.Collections.Generic;

namespace Raziel.Pure.Deck
{
    public class Deck
    {
        private readonly List<Card> _decks = new();
        public IReadOnlyList<Card> Decks => _decks;

        public void AddCard(Card card)
        {
            _decks.Add(card);
        }
    }
}