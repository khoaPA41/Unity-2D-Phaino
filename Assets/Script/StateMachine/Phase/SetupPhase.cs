using UnityEngine;

namespace Raziel.StateMachine
{
    public class SetupPhase : DuelBaseState
    {
        DuelStateMachine _stateMachine;
        public SetupPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            Debug.Log("SetupPhase");

            _stateMachine = _duelStateMachine;

            _stateMachine.TurnController.BuildInitial(_stateMachine.PlayerDuelInformation, _stateMachine.EnemyDuelInformation);
            _stateMachine.CurrentTurn = _stateMachine.TurnController.GetCurrentTurn();

            SetupPlayerDeck();
            SetupEnemyDeck();

            DrawFirstTime();

            _stateMachine.SwitchState(_stateMachine.StandbyPhase);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            _stateMachine = null;
        }

        private void SetupPlayerDeck()
        {
            _stateMachine.PlayerDuelInformation.SetupDeck();
        }

        private void SetupEnemyDeck()
        {
            _stateMachine.EnemyDuelInformation.SetupDeck();
        }

        private void DrawFirstTime()
        {
            _stateMachine.PlayerDuelInformation.DrawCardFirstTimes();
            _stateMachine.EnemyDuelInformation.DrawCardFirstTimes();
        }
    }
}