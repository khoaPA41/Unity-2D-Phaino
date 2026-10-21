using Raziel.Battle;
using UnityEngine;

namespace Raziel.StateMachine
{
    public class ResultPhase : DuelBaseState
    {
        private DuelStateMachine _stateMachine;
        BattleBoard _battleBoard;
        public ResultPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _stateMachine = _duelStateMachine;
            _battleBoard = BattleBoard.Instance;



        }

        public override void Exit()
        {
        }

        public override void Tick(float deltaTime)
        {
        }

        private void CheckFragmentSoul()
        {
            // _duelStateMachine.PlayerDuelInformation
        }
    }
}