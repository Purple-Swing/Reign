using System.Collections.Generic;
using Reign.Generic.UI.Subclasses;
using UnityEngine;

namespace Reign.Generic.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private List<UIElement> elements = new();
        private Dictionary<string, UIElement> lookupTable = new();

        private void Awake()
        {
            Refresh();
        }

        public void Register(UIElement newElement)
        {
            if (newElement.Manager != this) return; // Don't allow a register if the manager isn't this.

            if (!elements.Contains(newElement))
            {
                elements.Add(newElement);   
            }

            if (!lookupTable.ContainsKey(newElement.Key))
            {
                lookupTable.Add(newElement.Key, newElement);
            }
        }

        public void Unregister(UIElement element)
        {
            elements.Remove(element);

            if (lookupTable.TryGetValue(element.Key, out UIElement found))
            {
                if (found == element) lookupTable.Remove(element.Key);
            }
        }

        public void Refresh()
        {
            elements.Clear();
            lookupTable.Clear();

            foreach (var element in GetComponentsInChildren<UIElement>(true))
            {
                if (element.Manager != this) continue;

                // Elements are everything the UI manager owns, so the lookup table is the only one that shouldn't
                // have duplicates.

                elements.Add(element);

                if (lookupTable.ContainsKey(element.Key))
                {
                    Debug.LogWarning($"Duplicate key of {element.Key} already exists, ignoring the new key.");
                    continue;
                }

                lookupTable.Add(element.Key, element);
            }
        }
        
        public bool Find(string key, out UIElement element)
        {
            if (!lookupTable.TryGetValue(key, out element))
            {
                Debug.LogWarning($"UI element of key '{key}' could not be found.");
                return false;
            }

            return true;
        }

        public T GetElement<T>(string key) where T : UIElement
        {
            if (!lookupTable.TryGetValue(key, out var element))
            {
                Debug.LogWarning($"UI element of key '{key}' could not be found.");
                return null;
            }

            if (element is T typed)
            {
                return typed;
            }

            Debug.LogWarning($"Element is not of type {typeof(T).Name}.");
            return null;
        }
    }
}
