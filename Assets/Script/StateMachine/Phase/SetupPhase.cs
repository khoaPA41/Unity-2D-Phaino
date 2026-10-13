using UnityEngine;

namespace Raziel.StateMachine
{
    public class SetupPhase : DuelBaseState
    {

        public SetupPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            Debug.Log("SetupPhase");

            _duelStateMachine.TurnController.BuildInitial(_duelStateMachine.DuelInformation_I, _duelStateMachine.DuelInformation_II);
            _duelStateMachine.CurrentTurn = _duelStateMachine.TurnController.GetCurrentTurn();
            SetupPlayerDeck();
            SetupEnemyDeck();
            _duelStateMachine.SwitchState(_duelStateMachine.DrawPhase);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }

        private void SetupPlayerDeck()
        {
            _duelStateMachine.DuelInformation_I.SetupDeck();
        }

        private void SetupEnemyDeck()
        {
            _duelStateMachine.DuelInformation_II.SetupDeck();
        }
    }
}