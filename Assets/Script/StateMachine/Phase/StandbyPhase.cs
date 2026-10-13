using UnityEngine;

namespace Raziel.StateMachine
{
    public class StandbyPhase : DuelBaseState
    {
        public StandbyPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            Debug.Log("StandbyPhase: " + _duelStateMachine.CurrentTurn.character);

            _duelStateMachine.SwitchState(_duelStateMachine.FirstMainPhase);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}