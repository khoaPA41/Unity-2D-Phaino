using Mono.Cecil.Cil;
using Raziel.Battle;
using UnityEngine;

namespace Raziel.StateMachine
{
    public class DrawPhase : DuelBaseState
    {
        DuelStateMachine _stateMachine;
        DuelInformation _current;
        UiBattleManagers _uiBattleManagers;

        public DrawPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _stateMachine = _duelStateMachine;
            _uiBattleManagers = UiBattleManagers.Instance;
            _stateMachine.DrawButton.SetActive(true);


            Debug.Log("DrawPhase: " + _stateMachine.CurrentTurn.character);

            _current = _stateMachine.CurrentTurn;
            ActionByCharacter();
            ChangePhaseAfterDraw();


        }

        public override void Tick(float deltaTime)
        {


        }

        public override void Exit()
        {
            if (_current != null)
            {
                _current.DrawCardSucces -= ChangeMain_I;
                _current.DrawCardSucces -= ChangeMain_II;
            }

            if (_stateMachine != null)
            {
                _stateMachine.DrawButton.SetActive(false);
                _stateMachine.DrawCardAction -= _current.DrawCard;
            }

            _stateMachine = null;
            _current = null;
        }

        private void ActionByCharacter()
        {
            _stateMachine.DrawCardAction += _current.DrawCard;
        }

        private void ChangePhaseAfterDraw()
        {
            if (_stateMachine.Turn < _stateMachine.TwoFirstTurnBattle)
            {
                _current.DrawCardSucces += ChangeMain_II;
                _stateMachine.Turn += 1;
            }
            else
            {
                _current.DrawCardSucces += ChangeMain_I;
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

        private void UpdateCardHand()
        {
            // _current.UpdateCardHand += () => _uiBattleManagers.SpawnCardElementOnHand();
        }
    }
}