using UnityEngine;

namespace Raziel.StateMachine
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Tick(float deltaTime);
        public abstract void Exit();

        public float NormalizedTime(Animator animator, string animationTag)
        {
            var currentState = animator.GetCurrentAnimatorStateInfo(0);
            var nextState = animator.GetNextAnimatorStateInfo(0);

            if (nextState.IsTag(animationTag) && animator.IsInTransition(0))
            {
                return nextState.normalizedTime;
            }
            else if (currentState.IsTag(animationTag) && !animator.IsInTransition(0))
            {
                return currentState.normalizedTime;
            }
            else
            {
                return 0f;
            }
        }
    }
}

