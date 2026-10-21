using UnityEngine;

namespace Raziel.StateMachine
{
    public class StandbyPhase : DuelBaseState
    {
        public StandbyPhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            Debug.Log("StandbyPhase: " + _duelStateMachine.CurrentTurn.character);

            CheckDefCardByCharacter();

            _duelStateMachine.SwitchState(_duelStateMachine.DrawPhase);
        }

        public override void Tick(float deltaTime)
        {
            // Hien UI thong bao turn cua ai
        }

        public override void Exit()
        {

        }

        private void CheckDefCardByCharacter()
        {
            _duelStateMachine.BattleBoard.ProcessDefenseElapsed(_duelStateMachine.CurrentTurn.character);
        }
    }
}