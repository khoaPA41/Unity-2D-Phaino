using UnityEngine;
namespace Raziel.StateMachine
{
    public class BattlePhase : DuelBaseState
    {
        public BattlePhase(DuelStateMachine duelStateMachine) : base(duelStateMachine)
        {
        }

        public override void Enter()
        {
            Debug.Log("BattlePhase: " + _duelStateMachine.CurrentTurn.character);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}