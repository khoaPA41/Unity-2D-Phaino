using UnityEngine;

namespace Raziel.StateMachine
{
    public class StandbyPhase : DuelBaseState
    {
        DuelStateMachine _stateMachine;
        public StandbyPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _stateMachine = _duelStateMachine;
            Debug.Log("StandbyPhase: " + _duelStateMachine.CurrentTurn.character);

            _stateMachine.SwitchState(_duelStateMachine.DrawPhase);
        }

        public override void Tick(float deltaTime)
        {
            // Hien UI thong bao turn cua ai
        }

        public override void Exit()
        {
            _stateMachine = null;
        }
    }
}