using Raziel.Battle;
using UnityEngine;
namespace Raziel.StateMachine
{
    public class BattlePhase : DuelBaseState
    {
        BattleBoard _battleBoard;


        public BattlePhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _battleBoard = _duelStateMachine.BattleBoard;

            Debug.Log("BattlePhase: " + _duelStateMachine.CurrentTurn.character);

            // Lam nut chuyen
            //Tam thoi cho tu chuyen

            _battleBoard.CardOnBoardClicked += GetCard;
            ChangePhaseAfterBattle();

            SetupForPlayer();
            SetupForEnemy();
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            _duelStateMachine.MainIIButton.SetActive(false);
            _duelStateMachine.AttackButton.SetActive(false);
            _duelStateMachine.MainIIAction -= ChangeMainII;

            if (_battleBoard != null)
            {
                _battleBoard.CardOnBoardClicked -= GetCard;
                _battleBoard.SetTarget(null);
                _battleBoard.SetAttacker(null);
            }

            _battleBoard = null;
        }

        private void ChangePhaseAfterBattle()
        {
            _duelStateMachine.MainIIAction += ChangeMainII;
        }

        private void ChangeMainII()
        {
            _duelStateMachine.SwitchState(_duelStateMachine.SecondMainPhase);
        }

        private void GetCard(BoardSlot boardSlot)
        {
            if (!boardSlot.AcceptSoul) return;
            if (_duelStateMachine.CurrentTurn.character == boardSlot.Character)
            {
                _battleBoard.SetAttacker(boardSlot);
            }
            else
            {
                _battleBoard.SetTarget(boardSlot);
            }
        }

        private void SetupForPlayer()
        {
            if (_duelStateMachine.CurrentTurn.character != Character.Player) return;
            _duelStateMachine.MainIIButton.SetActive(true);
            _duelStateMachine.AttackButton.SetActive(true);
        }

        private void SetupForEnemy()
        {
            if (_duelStateMachine.CurrentTurn.character != Character.Enemy) return;

            _duelStateMachine.EnemyInput.SetInput(EnemyInput.Attack);

            _duelStateMachine.EnemyInput.SetInput(EnemyInput.Main_II);
        }
    }
}