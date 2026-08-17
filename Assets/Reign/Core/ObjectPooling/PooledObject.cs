using UnityEngine;

namespace Reign.Core.ObjectPooling
{
    public class PooledObject : MonoBehaviour
    {
        private GameObject prefab;

        public void Initialize(GameObject prefab)
        {
            this.prefab = prefab;
        }

        public void Release()
        {
            ObjectPoolManager.Return(prefab, gameObject);
        }

        public virtual void OnGet()
        {
            
        }

        public virtual void OnRelease()
        {
            
        }
    }
}