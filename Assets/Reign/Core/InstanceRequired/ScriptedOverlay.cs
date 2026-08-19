using System;
using System.Threading.Tasks;
using Reign.Core.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Reign.Core.InstanceRequired
{
    public class ScriptedOverlay : Singleton<ScriptedOverlay>
    {
        [SerializeField]
        private Image overlay;

        public async Task Flash(Color color, float duration = 1.0f)
        {
            overlay.color = color;

            while (overlay.color.a > 0f)
            {
                await Task.Yield();

                float alpha = Mathf.MoveTowards(overlay.color.a, 0.0f, Time.unscaledDeltaTime / duration);

                Color current = overlay.color;
                current.a = alpha;
                overlay.color = current;
            }
        }
    }
}
