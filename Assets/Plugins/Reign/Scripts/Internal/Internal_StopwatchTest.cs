using Reign.Generic.Timing;
using UnityEngine;

namespace Reign.Internal
{
    public class Internal_StopwatchTest : MonoBehaviour
    {
        Stopwatch stopwatchTest;

        private void Awake()
        {
            stopwatchTest = Stopwatch.Create(gameObject);
        }

        private void Update()
        {
            Debug.Log($"Elapsed: {stopwatchTest.ElapsedTime}");
        }
    }
}
