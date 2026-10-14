using UnityEngine;
namespace Raziel.StateMachine
{
    public class BattlePhase : DuelBaseState
    {
        DuelStateMachine _stateMachine;

        public BattlePhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _stateMachine = _duelStateMachine;




            Debug.Log("BattlePhase: " + _stateMachine.CurrentTurn.character);

            // Lam nut chuyen
            //Tam thoi cho tu chuyen
            ChangePhaseAfterBattle();

            _stateMachine.MainIIButton.SetActive(true);
        }

        public override void Tick(float deltaTime)
        {
            // _stateMachine = null;
        }

        public override void Exit()
        {
            _stateMachine.MainIIButton.SetActive(false);

            if (_stateMachine != null)
                _stateMachine.MainIIAction -= ChangeMainII;

            _stateMachine = null;
        }

        private void ChangePhaseAfterBattle()
        {
            _stateMachine.MainIIAction += ChangeMainII;
        }

        private void ChangeMainII()
        {
            _stateMachine.SwitchState(_stateMachine.SecondMainPhase);
        }
    }
}