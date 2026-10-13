using System.Collections.Generic;
using Raziel.Battle;
using UnityEngine;

namespace Raziel.Pure.TurnController
{
    public class TurnController
    {
        private readonly List<DuelInformation> _queue = new();

        public IReadOnlyList<DuelInformation> TurnList => _queue;


        public void BuildInitial(DuelInformation player, DuelInformation enemy)
        {
            if (_queue.Count > 0)
                _queue.Clear();
            // _queue.AddRange(turnList);
            _queue.Add(player);
            _queue.Add(enemy);
        }

        public DuelInformation GetCurrentTurn()
        {
            var currentTurn = _queue[0];
            _queue.RemoveAt(0);
            foreach (var item in _queue)
            {
                Debug.Log(item.character);
            }
            return currentTurn;
        }

        public void EnqueueBack(DuelInformation duelInformation)
        {
            _queue.Add(duelInformation);
        }
    }
}

