using System.Collections.Generic;
using UnityEngine;

namespace Reign.Generic
{
    public class ReignMonoBehaviour : MonoBehaviour
    {
        public readonly bool TrackInternalTime = false;
        public float InternalTimer { get; private set; } = 0.0f;

        private void Update()
        {
            if (!TrackInternalTime) return;
            InternalTimer += Time.unscaledDeltaTime;
        }

        /// <summary>
        /// Get a list children of the Transform the ReignMonoBehaviour is attached to
        /// </summary>
        public List<Transform> GetChildren()
        {
            var childCount = transform.childCount;
            List<Transform> children = new();

            for (int i = 0; i < childCount; ++i)
            {
                children.Add(transform.GetChild(i));
            }

            return children;
        }
    }
}

