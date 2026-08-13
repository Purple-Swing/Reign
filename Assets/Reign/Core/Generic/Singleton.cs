using UnityEngine;

namespace Reign.Core.Generic
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private bool destroyOnLoad = true;

        private static T current;

        public static T Current 
        {
            get
            {
                if (!Application.isPlaying) 
                { 
                    return null; 
                }

                if (current == null)
                {
                    current = FindAnyObjectByType<T>();
                }

                return current;
            }
        }

        protected virtual void Awake()
        {
            if (current != null && current != this)
            {
                Destroy(gameObject);
                return;
            }

            current = this as T;

            if (!destroyOnLoad) DontDestroyOnLoad(gameObject);
        }
    }
}