using Raziel.Battle;
using UnityEngine;

namespace Raziel.StateMachine
{
    public class SecondMainPhase : DuelBaseState
    {
        DuelStateMachine _stateMachine;
        DuelInformation _duelInformation;
        BattleBoard _battleBoard;
        public SecondMainPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _stateMachine = _duelStateMachine;
            _duelInformation = _duelStateMachine.CurrentTurn;
            _battleBoard = BattleBoard.Instance;

            _stateMachine.EndButton.SetActive(true);
            _stateMachine.SummonButton.SetActive(true);

            Debug.Log("SecondMainPhase: " + _stateMachine.CurrentTurn.character);
            ChangePhaseAfterMain_II();

            ChooseCard();
            _stateMachine.SummonAction += Summon;
        }

        public override void Tick(float deltaTime)
        {

        }

        public override void Exit()
        {
            if (_stateMachine != null)
            {
                _stateMachine.EndButton.SetActive(false);
                _stateMachine.SummonButton.SetActive(false);
                _stateMachine.EndAction -= ChangeEnd;
                _stateMachine.SummonAction -= Summon;

            }

            _stateMachine = null;
            _duelInformation = null;
            _battleBoard = null;
        }

        private void ChangePhaseAfterMain_II()
        {
            _stateMachine.EndAction += ChangeEnd;
        }

        private void ChangeEnd()
        {
            _stateMachine.SwitchState(_stateMachine.EndPhase);
        }

        private void ChooseCard()
        {
            _duelInformation.ChooseCard(_duelInformation.index);
        }

        private void Summon()
        {
            _duelInformation.Summon();
        }
    }
}