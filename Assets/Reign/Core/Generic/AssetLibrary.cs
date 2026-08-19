using System.Collections.Generic;
using UnityEngine;

namespace Reign.Core.Generic
{
    [System.Serializable]
    public class AssetEntry<T> where T : Object
    {
        public string id;
        public T asset;
    }

    public abstract class AssetLibrary<T> : MonoBehaviour where T : Object
    {
        [SerializeField]
        private List<AssetEntry<T>> assetList;

        private Dictionary<string, T> assetDictionary;

        public void Refresh()
        {
            foreach (var entry in assetList)
            {
                // For each given entry, add with ID and asset.
                assetDictionary.TryAdd(entry.id, entry.asset);
            }
        }

        /// <summary>
        /// Find object of type T with the name of ID.
        /// </summary>
        public T Find(string id)
        {
            if(assetDictionary.TryGetValue(id, out var found))
            {
                // If asset dictionary contains asset with ID, return it.
                return found;
            }

            // Return the null equivalent of type T.
            return default;
        }

        public T GetValueAtIndex(int index)
        {
            return assetList[index].asset;
        }
    }
}
