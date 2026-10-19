using Raziel.Battle;
using UnityEngine;

namespace Raziel.StateMachine
{
    public class DrawPhase : DuelBaseState
    {
        DuelStateMachine _stateMachine;
        DuelInformation _current;

        public DrawPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _stateMachine = _duelStateMachine;

            Debug.Log("DrawPhase: " + _stateMachine.CurrentTurn.character);

            _current = _stateMachine.CurrentTurn;
            ListenAction();
            ChangePhaseAfterDraw();
            SetupForPlayer();
            SetupForEnemy();
        }

        public override void Tick(float deltaTime)
        {


        }

        public override void Exit()
        {
            if (_current != null)
            {
                _current.ChangesInHand -= ChangeMain_I;
                _current.ChangesInHand -= ChangeMain_II;
            }

            if (_stateMachine != null)
            {
                _stateMachine.DrawButton.SetActive(false);
                _stateMachine.DrawCardAction -= _current.DrawCard;
            }

            _stateMachine = null;
            _current = null;
        }

        private void ListenAction()
        {
            _stateMachine.DrawCardAction += _current.DrawCard;
        }

        private void ChangePhaseAfterDraw()
        {
            if (_stateMachine.Turn < _stateMachine.TwoFirstTurnBattle)
            {
                _current.ChangesInHand += ChangeMain_II;
                _stateMachine.Turn += 1;
            }
            else
            {
                _current.ChangesInHand += ChangeMain_I;
            }
        }

        private void ChangeMain_I()
        {
            _stateMachine.SwitchState(_stateMachine.FirstMainPhase);
        }

        private void ChangeMain_II()
        {
            _stateMachine.SwitchState(_stateMachine.SecondMainPhase);
        }

        private void SetupForPlayer()
        {
            if (_current.character == Character.Player)
                _stateMachine.DrawButton.SetActive(true);
        }

        private void SetupForEnemy()
        {
            if (_current.character == Character.Enemy)
                _stateMachine.Draw();
        }
    }
}