using UnityEngine;

namespace Raziel.StateMachine
{
    public class SecondMainPhase : DuelBaseState
    {
        DuelStateMachine _stateMachine;
        public SecondMainPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _stateMachine = _duelStateMachine;
            // _stateMachine.MainIIButton.SetActive(true);
            _stateMachine.EndButton.SetActive(true);

            Debug.Log("SecondMainPhase: " + _stateMachine.CurrentTurn.character);
            ChangePhaseAfterMain_II();
        }

        public override void Tick(float deltaTime)
        {

        }

        public override void Exit()
        {
            // _stateMachine.MainIIButton.SetActive(false);
            _stateMachine.EndButton.SetActive(false);

            if (_stateMachine != null)
                _stateMachine.EndAction -= ChangeEnd;

            _stateMachine = null;
        }

        private void ChangePhaseAfterMain_II()
        {
            _stateMachine.EndAction += ChangeEnd;
        }

        private void ChangeEnd()
        {
            _stateMachine.SwitchState(_stateMachine.EndPhase);
        }
    }
}