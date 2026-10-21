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
            _duelStateMachine.EndButton.SetActive(true);
            Debug.Log("EndPhase: " + _duelStateMachine.CurrentTurn.character);

            _duelStateMachine.CurrentTurn = _duelStateMachine.TurnController.GetCurrentTurn();
            _duelStateMachine.SwitchState(_duelStateMachine.StandbyPhase);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            _duelStateMachine.EndButton.SetActive(false);
        }
    }
}