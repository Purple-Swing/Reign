using System.Collections;
using UnityEngine;
using NaughtyAttributes;

namespace Reign.Generic.Shared
{
    public class TimedReignMonoBehaviour : ReignMonoBehaviour
    {
        public bool stopwatchActive;
        public bool scaledTime;
        [ShowNativeProperty] public float objectActiveTimer {get; private set;} = 0.0f;

        private void Update()
        {
            if (!stopwatchActive) return;

            objectActiveTimer += scaledTime ? Time.deltaTime : Time.unscaledDeltaTime;   
        }
    }
}