using System.Collections.Generic;
using UnityEngine;

namespace Reign.Core.ObjectPooling
{
    public static class ObjectPoolManager
    {
        private static readonly Dictionary<GameObject, Queue<GameObject>> pools = new();

        /// <summary>
        /// Create a new pool with an amount of objects.
        /// </summary>
        public static void CreatePoolOfPrefab(GameObject prefab, int size)
        {
            if (prefab == null || pools.ContainsKey(prefab))
            {
                return;
            }

            Queue<GameObject> newPool = new();

            for (int i = 0; i < size; ++i)
            {
                GameObject obj = CreateObject(prefab);
                newPool.Enqueue(obj);
            }

            pools.Add(prefab, newPool);
        }

        /// <summary>
        /// Get a pooled prefab.
        /// </summary>
        public static GameObject Get(GameObject prefab)
        {
            if (prefab == null)
            {
                return null;
            }

            if (!pools.ContainsKey(prefab))
            {
                CreatePoolOfPrefab(prefab, 1);
            }

            Queue<GameObject> pool = pools[prefab];

            GameObject obj;

            if (pool.Count > 0)
            {
                obj = pool.Dequeue();
            }
            else
            {
                obj = CreateObject(prefab);
            }

            obj.SetActive(true);

            obj.GetComponent<PooledObject>().OnGet();

            return obj;
        }

        /// <summary>
        /// Release an object back to its pool.
        /// </summary>
        public static void Release(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }

            if (!obj.TryGetComponent<PooledObject>(out var pooledObject))
            {
                Object.Destroy(obj);
                return;
            }

            pooledObject.OnRelease();
            pooledObject.Release();
        }

        private static GameObject CreateObject(GameObject prefab)
        {
            GameObject obj = Object.Instantiate(prefab);
            obj.SetActive(false);

            PooledObject pooledObject = obj.GetComponent<PooledObject>();

            if (pooledObject == null)
            {
                pooledObject = obj.AddComponent<PooledObject>();
            }

            pooledObject.Initialize(prefab);

            return obj;
        }

        /// <summary>
        /// Enqueue a prefab back to its pool.
        /// </summary>
        public static void Return(GameObject prefab, GameObject obj)
        {
            if (prefab == null || obj == null)
            {
                return;
            }

            if (!pools.TryGetValue(prefab, out Queue<GameObject> pool))
            {
                Object.Destroy(obj);
                return;
            }

            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        /// <summary>
        /// Clear pool of a prefab.
        /// </summary>
        public static void Clear(GameObject prefab)
        {
            if (!pools.TryGetValue(prefab, out Queue<GameObject> pool))
            {
                return;
            }

            while (pool.Count > 0)
            {
                Object.Destroy(pool.Dequeue());
            }

            pools.Remove(prefab);
        }

        /// <summary>
        /// Clear every pool.
        /// </summary>
        public static void ClearAll()
        {
            foreach (Queue<GameObject> pool in pools.Values)
            {
                while (pool.Count > 0)
                {
                    Object.Destroy(pool.Dequeue());
                }
            }

            pools.Clear();
        }
    }
}