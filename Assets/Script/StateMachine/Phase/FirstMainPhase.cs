using Raziel.Battle;
using UnityEngine;

namespace Raziel.StateMachine
{
    public class FirstMainPhase : DuelBaseState
    {
        public FirstMainPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _duelStateMachine.IsCanUseCard = true;

            Debug.Log("FirstMainPhase: " + _duelStateMachine.CurrentTurn.character);

            _duelStateMachine.BattleAction += ChangeBattle;
            SetupForPlayer();
            SetupForEnemy();
        }

        public override void Tick(float deltaTime)
        {
            // Hien nut battle

        }

        public override void Exit()
        {
            _duelStateMachine.BattleButton.SetActive(false);
            _duelStateMachine.IsCanUseCard = false;
            _duelStateMachine.BattleAction -= ChangeBattle;
        }

        private void ChangeBattle()
        {
            _duelStateMachine.SwitchState(_duelStateMachine.BattlePhase);
        }

        private void SetupForPlayer()
        {
            if (_duelStateMachine.CurrentTurn.character != Character.Player) return;
            _duelStateMachine.BattleButton.SetActive(true);
        }

        private void SetupForEnemy()
        {
            if (_duelStateMachine.CurrentTurn.character != Character.Enemy) return;
            _duelStateMachine.EnemyInput.SetInput(EnemyInput.Battle);
        }
    }
}