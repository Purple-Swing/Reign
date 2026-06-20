using System.Collections.Generic;
using UnityEngine;

namespace Reign.Generic.Shared
{
    public class ReignMonoBehaviour : MonoBehaviour
    {
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

