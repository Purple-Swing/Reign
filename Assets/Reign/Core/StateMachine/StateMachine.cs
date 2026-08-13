using UnityEngine;

namespace Reign.Core.StateMachine
{
    public class StateMachine<TOwner>
    {
        public TOwner Owner { get; }

        public float CurrentStateTime { get; private set; }
        public State<TOwner> CurrentState { get; private set; }
        public State<TOwner> LastState { get; private set; }

        public StateMachine(TOwner owner)
        {
            Owner = owner;
        }

        public void ChangeState(State<TOwner> state)
        {
            if (CurrentState != null)
            {
                if (!CurrentState.CanLeave() || CurrentState == state) return;
            }

            LastState = CurrentState;

            CurrentState?.StateLeave();

            CurrentState = state;

            CurrentState.StateEnter();

            CurrentStateTime = 0.0f;
        }

        public void Tick(float deltaTime)
        {
            if (CurrentState == null) return;

            CurrentStateTime += deltaTime;
            CurrentState.StateTick(deltaTime);
        }
    }
}
