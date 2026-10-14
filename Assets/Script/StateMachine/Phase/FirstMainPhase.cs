using UnityEngine;

namespace Raziel.StateMachine
{
    public class FirstMainPhase : DuelBaseState
    {
        DuelStateMachine _stateMachine;

        public FirstMainPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _stateMachine = _duelStateMachine;
            _stateMachine.BattleButton.SetActive(true);
            Debug.Log("FirstMainPhase: " + _stateMachine.CurrentTurn.character);

            ChangePhaseAfterMain_I();
        }

        public override void Tick(float deltaTime)
        {
            // Hien nut battle

        }

        public override void Exit()
        {
            _stateMachine.BattleButton.SetActive(false);
            if (_stateMachine != null)
                _stateMachine.BattleAction -= ChangeBattle;
        }

        private void ChangePhaseAfterMain_I()
        {
            _stateMachine.BattleAction += ChangeBattle;
        }

        private void ChangeBattle()
        {
            _stateMachine.SwitchState(_stateMachine.BattlePhase);
        }

    }
}