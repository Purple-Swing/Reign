using UnityEngine;

namespace Reign.Core.StateMachine
{
    public abstract class State<TOwner> : IState
    {
        protected TOwner Owner { get; }

        protected State(TOwner owner)
        {
            Owner = owner;
        }

        public virtual void StateEnter() { }
        public virtual void StateTick(float deltaTime) { }
        
        public virtual bool CanLeave() { return true; }
        public virtual void StateLeave() { }
    }
}
