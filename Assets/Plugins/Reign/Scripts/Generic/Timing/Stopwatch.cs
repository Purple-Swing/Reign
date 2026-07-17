using UnityEngine;

namespace Reign.Generic.Timing
{
    public class Stopwatch : MonoBehaviour
    {
        public float ElapsedTime
        {
            get
            {
                return elapsed;
            }
        }

        public bool IsRunning
        {
            get
            {
                return running;
            }
        }

        public static Stopwatch Create(GameObject obj, float startOffsetTime = 0.0f, bool startRunning = true, bool isUnscaled = false)
        {
            var sw = obj.AddComponent<Stopwatch>();
            sw.InitialiseStopwatch(startOffsetTime, startRunning, isUnscaled);
            return sw;
        }

        private void InitialiseStopwatch(float startOffsetTime, bool startRunning, bool isUnscaled)
        {
            running = startRunning;
            elapsed = startOffsetTime;

            unscaled = isUnscaled; // Only set scaled/unscaled on construction
        }

        public void SetStopwatchRunning(bool isRunning)
        {
            running = isRunning;
        }

        public void SetStopwatchElapsedTime(float time)
        {
            elapsed = time;
        }

        // -------------------------------------------------------------------------

        private bool unscaled = false;
        private float elapsed = 0.0f;
        private bool running = false;

        private void Update()
        {
            if (!running) return;

            elapsed += unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
        }
    }
}