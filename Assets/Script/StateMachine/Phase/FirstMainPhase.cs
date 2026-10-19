using Raziel.Battle;
using UnityEngine;

namespace Raziel.StateMachine
{
    public class FirstMainPhase : DuelBaseState
    {
        DuelStateMachine _stateMachine;
        DuelInformation _currentTurn;
        public FirstMainPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _stateMachine = _duelStateMachine;
            _currentTurn = _stateMachine.CurrentTurn;

            _stateMachine.IsCanUseCard = true;

            Debug.Log("FirstMainPhase: " + _stateMachine.CurrentTurn.character);

            _stateMachine.BattleAction += ChangeBattle;
            SetupForPlayer();
            SetupForEnemy();
        }

        public override void Tick(float deltaTime)
        {
            // Hien nut battle

        }

        public override void Exit()
        {
            if (_stateMachine != null)
            {
                _stateMachine.BattleButton.SetActive(false);
                _stateMachine.IsCanUseCard = false;
                _stateMachine.BattleAction -= ChangeBattle;
            }

            _stateMachine = null;
        }

        private void ChangeBattle()
        {
            _stateMachine.SwitchState(_stateMachine.BattlePhase);
        }

        private void SetupForPlayer()
        {
            if (_currentTurn.character != Character.Player) return;
            _stateMachine.BattleButton.SetActive(true);
        }

        private void SetupForEnemy()
        {
            if (_currentTurn.character != Character.Enemy) return;
            _stateMachine.EnemyInput.SetInput(EnemyInput.Battle);
        }
    }
}