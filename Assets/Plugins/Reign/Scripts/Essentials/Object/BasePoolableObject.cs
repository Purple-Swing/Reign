using Reign.Generic.Shared;
using Reign.Interfaces;

namespace Reign.Essentials
{
    public class BasePoolableObject : ReignMonoBehaviour, IPoolable
    {
        public virtual void OnReclaim()
        {
        }

        public virtual void OnReset()
        {
        }

        public virtual void OnSpawn()
        {
        }
    }
}