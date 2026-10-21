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

            _duelStateMachine.TurnController.BuildInitial(_duelStateMachine.PlayerDuelInformation, _duelStateMachine.EnemyDuelInformation);
            _duelStateMachine.CurrentTurn = _duelStateMachine.TurnController.GetCurrentTurn();

            SetupPlayerDeck();
            SetupEnemyDeck();

            DrawFirstTime();

            _duelStateMachine.SwitchState(_duelStateMachine.StandbyPhase);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }

        private void SetupPlayerDeck()
        {
            _duelStateMachine.PlayerDuelInformation.SetupDeck();
        }

        private void SetupEnemyDeck()
        {
            _duelStateMachine.EnemyDuelInformation.SetupDeck();
        }

        private void DrawFirstTime()
        {
            _duelStateMachine.PlayerDuelInformation.DrawCardFirstTimes();
            _duelStateMachine.EnemyDuelInformation.DrawCardFirstTimes();
        }
    }
}