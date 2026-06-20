using System.Collections;
using UnityEngine;
using NaughtyAttributes;

namespace Reign.Generic.Shared
{
    public class TimeMeasurementObject : ReignMonoBehaviour
    {
        public bool timerActive;
        public bool scaledTime;
        [ShowNativeProperty] public float objectActiveTimer {get; private set;} = 0.0f;

        private void Update()
        {
            if (!timerActive) return;

            objectActiveTimer += scaledTime ? Time.deltaTime : Time.unscaledDeltaTime;   
        }
    }
}