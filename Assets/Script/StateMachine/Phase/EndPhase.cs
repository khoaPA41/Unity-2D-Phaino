using UnityEngine;

namespace Raziel.StateMachine
{
    public class EndPhase : DuelBaseState
    {
        public EndPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            Debug.Log("EndPhase: " + _duelStateMachine.CurrentTurn.character);
            _duelStateMachine.TurnController.EnqueueBack(_duelStateMachine.CurrentTurn);
            _duelStateMachine.CurrentTurn = _duelStateMachine.TurnController.GetCurrentTurn();
            _duelStateMachine.SwitchState(_duelStateMachine.StandbyPhase);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}