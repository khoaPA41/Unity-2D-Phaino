using Raziel.Battle;
using UnityEngine;
namespace Raziel.StateMachine
{
    public class BattlePhase : DuelBaseState
    {
        DuelStateMachine _stateMachine;
        DuelInformation _currentTurn;
        BattleBoard _battleBoard;


        public BattlePhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _stateMachine = _duelStateMachine;
            _currentTurn = _stateMachine.CurrentTurn;
            _battleBoard = BattleBoard.Instance;

            Debug.Log("BattlePhase: " + _stateMachine.CurrentTurn.character);

            // Lam nut chuyen
            //Tam thoi cho tu chuyen

            _battleBoard.CardOnBoardClicked += GetCard;
            ChangePhaseAfterBattle();

            SetupForPlayer();
            SetupForEnemy();

        }

        public override void Tick(float deltaTime)
        {
            // _stateMachine = null;
        }

        public override void Exit()
        {
            if (_stateMachine != null)
            {
                _stateMachine.MainIIButton.SetActive(false);
                _stateMachine.AttackButton.SetActive(false);
                _stateMachine.MainIIAction -= ChangeMainII;
            }

            if (_battleBoard != null)
            {
                _battleBoard.CardOnBoardClicked -= GetCard;
                _battleBoard.SetTarget(null);
                _battleBoard.SetAttacker(null);
            }

            _stateMachine = null;
            _battleBoard = null;
            _currentTurn = null;
        }

        private void ChangePhaseAfterBattle()
        {
            _stateMachine.MainIIAction += ChangeMainII;
        }

        private void ChangeMainII()
        {
            _stateMachine.SwitchState(_stateMachine.SecondMainPhase);
        }


        private void GetCard(BoardSlot boardSlot)
        {
            if (!boardSlot.AcceptSoul) return;
            if (_currentTurn.character == boardSlot.Character)
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
            if (_stateMachine.CurrentTurn.character != Character.Player) return;
            _stateMachine.MainIIButton.SetActive(true);
            _stateMachine.AttackButton.SetActive(true);

        }

        private void SetupForEnemy()
        {
            if (_stateMachine.CurrentTurn.character != Character.Enemy) return;

            _stateMachine.EnemyInput.SetInput(EnemyInput.Attack);

            _stateMachine.EnemyInput.SetInput(EnemyInput.Main_II);
        }
    }
}