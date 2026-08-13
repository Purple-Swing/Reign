using UnityEngine;

namespace Reign.Core.StateMachine
{
    public interface IState
    {
        void StateEnter();
        void StateTick(float deltaTime);
        void StateLeave();
    }
}
