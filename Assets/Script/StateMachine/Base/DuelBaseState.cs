namespace Raziel.StateMachine
{
    public abstract class DuelBaseState : State
    {
        protected DuelStateMachine _duelStateMachine;
        protected DuelBaseState(DuelStateMachine duelStateMachine)
        {
            _duelStateMachine = duelStateMachine;
        }
    }
}