using System.Collections;
using Reign.Generic.Shared;
using UnityEngine;

namespace Reign.Systems
{
    public abstract class System<T> : Singleton<T> where T : ReignMonoBehaviour
    {
        #if UNITY_EDITOR
        
        [SerializeField] private bool logInfo;

        private void Start()
        {
            if (!logInfo) return;
            Debug.Log($"System (type {typeof(T)}) initialised.");
        } 

        #endif
    }
}