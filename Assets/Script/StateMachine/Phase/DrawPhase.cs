using UnityEngine;

namespace Raziel.StateMachine
{
    public class DrawPhase : DuelBaseState
    {
        public DrawPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            Debug.Log("DrawPhase: " + _duelStateMachine.CurrentTurn.character);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}