using UnityEngine;

namespace Raziel.StateMachine
{
    public class SecondMainPhase : DuelBaseState
    {
        public SecondMainPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            Debug.Log("SecondMainPhase: " + _duelStateMachine.CurrentTurn.character);

        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}