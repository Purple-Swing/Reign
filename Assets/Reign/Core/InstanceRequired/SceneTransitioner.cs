using Reign.Core.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Reign.Core.InstanceRequired
{
    public class SceneTransitioner : Singleton<SceneTransitioner>
    {
        [SerializeField]
        private Image transitionOverlay;

        public enum TransitionFadeDirection
        {
            In,
            Out
        }

        public async Task Fade(TransitionFadeDirection dir, float duration)
        {
            float target = dir == TransitionFadeDirection.In ? 1.0f : 0.0f;

            Color color = transitionOverlay.color;

            while (!Mathf.Approximately(color.a, target))
            {
                await Task.Yield();

                color = transitionOverlay.color;

                float newAlpha = Mathf.MoveTowards(color.a, target, Time.unscaledDeltaTime * duration);

                transitionOverlay.color = new Color(color.r, color.g, color.b, newAlpha);
            }
        }
    }
}
