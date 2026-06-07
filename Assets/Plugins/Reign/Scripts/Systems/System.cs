using System.Collections;
using Reign.Generic;
using UnityEngine;

namespace Reign.Systems
{
    public abstract class SystemBase : ReignMonoBehaviour { }

    public abstract class System<T> : SystemBase where T : System<T>
    {
        // System<T> is its own Singleton, making it inherit Singleton<T> directly has problems in Reign.cs

        [SerializeField] bool dontDestroyOnLoad;

        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance != null) return instance;

                if (!Application.isPlaying) return null;

                instance = FindAnyObjectByType<T>();
                return instance;
            }
        }

        IEnumerator Start()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                yield break;
            }

            instance = this as T;

            if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
    }
}