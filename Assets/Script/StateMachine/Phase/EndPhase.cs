using UnityEngine;

namespace Raziel.StateMachine
{
    public class EndPhase : DuelBaseState
    {
        DuelStateMachine _stateMachine;
        public EndPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _stateMachine = _duelStateMachine;
            _stateMachine.EndButton.SetActive(true);
            Debug.Log("EndPhase: " + _stateMachine.CurrentTurn.character);

            _stateMachine.CurrentTurn = _stateMachine.TurnController.GetCurrentTurn();
            _stateMachine.SwitchState(_stateMachine.StandbyPhase);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            _stateMachine.EndButton.SetActive(false);

            _stateMachine = null;
        }
    }
}