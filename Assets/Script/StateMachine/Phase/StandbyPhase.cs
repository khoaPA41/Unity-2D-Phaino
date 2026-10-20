using Raziel.Battle;
using UnityEngine;

namespace Raziel.StateMachine
{
    public class StandbyPhase : DuelBaseState
    {
        DuelStateMachine _stateMachine;
        BattleBoard _battleBoard;
        public StandbyPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            _stateMachine = _duelStateMachine;
            _battleBoard = BattleBoard.Instance;

            Debug.Log("StandbyPhase: " + _stateMachine.CurrentTurn.character);

            CheckDefCardByCharacter();
            _stateMachine.SwitchState(_duelStateMachine.DrawPhase);
        }

        public override void Tick(float deltaTime)
        {
            // Hien UI thong bao turn cua ai
        }

        public override void Exit()
        {
            _stateMachine = null;
            _battleBoard = null;
        }

        private void CheckDefCardByCharacter()
        {
            _battleBoard.ProcessDefenseElapsed(_stateMachine.CurrentTurn.character);
        }
    }
}