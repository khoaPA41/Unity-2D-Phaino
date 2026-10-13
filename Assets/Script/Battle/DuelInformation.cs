using System.Collections.Generic;
using Raziel.Pure.Deck;
using UnityEngine;

namespace Raziel.Battle
{
    public enum Character
    {
        Player, Enemy
    }

    public class DuelInformation : MonoBehaviour
    {
        [SerializeField] private List<Card> _cards;
        public Character character;
        public Deck _deck;

        public void SetupDeck()
        {
            _deck = new();
            foreach (var card in _cards)
            {
                _deck.AddCard(card);
            }
        }
    }
}