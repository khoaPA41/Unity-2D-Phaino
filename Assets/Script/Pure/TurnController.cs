using System.Collections.Generic;
using Raziel.Battle;

namespace Raziel.Pure.TurnController
{
    public class TurnController
    {
        private readonly List<DuelInformation> _queue = new();

        public IReadOnlyList<DuelInformation> TurnList => _queue;

        private int currentIndex = 0;

        public void BuildInitial(DuelInformation player, DuelInformation enemy)
        {
            currentIndex = 0;
            if (_queue.Count > 0)
                _queue.Clear();

            _queue.Add(player);
            _queue.Add(enemy);
        }

        public DuelInformation GetCurrentTurn()
        {
            var currentTurn = _queue[currentIndex];

            var index = ++currentIndex;
            if (index >= _queue.Count) index = 0;

            currentIndex = index;

            return currentTurn;
        }
    }
}

