using UnityEngine;

namespace Raziel.StateMachine
{
    public class FirstMainPhase : DuelBaseState
    {
        public FirstMainPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            Debug.Log("FirstMainPhase: " + _duelStateMachine.CurrentTurn.character);

            // _duelStateMachine.SwitchState(_duelStateMachine.DrawPhase);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}