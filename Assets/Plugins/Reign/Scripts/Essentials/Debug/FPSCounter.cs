using System.Collections.Generic;
using Reign.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Threading.Tasks;

namespace Reign.Essentials
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private float updateInterval = 0.3f;
        [SerializeField] private bool trackAverage;
        [SerializeField, Label("GUI")] private bool gui = true;

        private readonly List<float> fpsSamples = new();
        private float fps;

        private float elapsed = 0.0f;
        private int frameCount = 0;

        private string displayText = "";

        private async void Start()
        {
            while (Reign.CurrentGameCertificates == null)
            {
                await Task.Yield();
            }
        }

        private void Update()
        {
            elapsed += Time.unscaledDeltaTime;
            frameCount++;

            if (elapsed >= updateInterval)
            {
                fps = frameCount / elapsed;

                if (trackAverage)
                {
                    // Remove oldest after 60 samples
                    if (fpsSamples.Count > 60)
                    {
                        fpsSamples.RemoveAt(0);
                    }

                    fpsSamples.Add(fps);
                }

                if (gui && Reign.CurrentGameCertificates.IS_DEBUG)
                {
                    // Change display text
                    displayText = trackAverage ? $"FPS: {fps:F1}\nAVERAGE: {GetAverageFPS(fpsSamples):F1}" : $"FPS: {fps:F1}";
                }

                frameCount = 0;
                elapsed = 0.0f;
            }
        }

        private void OnGUI()
        {
            if (!gui || !Reign.CurrentGameCertificates.IS_DEBUG) return;

            GUIStyle style = new(GUI.skin.label)
            {
                fontSize = 24,
                normal = { textColor = Color.white }
            };

            GUI.Box(new Rect(10, 10, 220, 100), GUIContent.none); // Background
            GUI.Label(new Rect(20, 20, 200, 80), displayText, style); // Text
        }

        private float GetAverageFPS(List<float> samples)
        {
            if (samples.Count == 0 || !trackAverage) return 0f;

            float total = 0f;
            foreach (float s in samples)
            {
                total += s;
            }

            return total / samples.Count;
        }
    }
}