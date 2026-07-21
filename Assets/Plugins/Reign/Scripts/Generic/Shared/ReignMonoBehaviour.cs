using System.Collections.Generic;
using UnityEngine;

namespace Reign.Generic.Shared
{
    public class ReignMonoBehaviour : MonoBehaviour
    {
        // Cached components
        public Transform Transform { get; private set; }

        private void Awake()
        {
            Transform = transform;
        }

        /// <summary>
        /// Get a list children of the Transform the ReignMonoBehaviour is attached to
        /// </summary>
        public List<Transform> GetChildren()
        {
            int childCount = transform.childCount;
            List<Transform> children = new(childCount);

            for (int i = 0; i < childCount; ++i)
            {
                children.Add(transform.GetChild(i));
            }

            return children;
        }
    }
}

