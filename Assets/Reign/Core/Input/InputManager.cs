using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reign.Core.Input
{
    public static class InputManager
    {
        public static InputActionMap Map { get; private set; }
        private static readonly Dictionary<string, InputAction> actions = new();

        public static InputAction FindOrAddAction(string name, bool tryAdd = true)
        {
            if (actions == null) return null;

            if (!actions.TryGetValue(name, out var action))
            {
                action = Map.FindAction(name);

                if (action == null)
                {
                    Debug.Log($"No input action for: {name}");
                    return null;
                }

                if (tryAdd)
                {
                    actions.Add(name, action);
                    Debug.Log($"Added input action with name '{name}' to the dictionary.");
                }
            }

            return action;
        }

        /// <summary>
        /// Find the action by name and return if it is being performed.
        /// </summary>
        public static bool GetButton(string name)
        {
            if (Map == null) return false;

            var action = FindOrAddAction(name);

            if (action == null) return false;

            // WasPerformedThisFrame() relies on the interactions given by the Input System itself.
            return action.WasPerformedThisFrame();
        }

        /// <summary>
        /// Get the vaue by name and return the current value.
        /// </summary>
        public static T GetValue<T>(string name) where T : struct
        {
            if (Map == null) return default;

            var action = FindOrAddAction(name);

            if (action != null)
            {
                return action.ReadValue<T>();
            }

            Debug.LogWarning($"Action '{name}' is invalid");
            return default;
        }

        /// <summary>
        /// Refresh the current map with a new input action asset and map name/
        /// </summary>
        public static void Refresh(InputActionAsset asset, string name)
        {
            var found = asset.FindActionMap(name, true);

            if (found != null && Map != found)
            {
                // One way gate, if the action map is found correctly and the current map is different to the found map:
                // Set the map to the found map.
                Map = found;
            }
        }
        
        /// <summary>
        /// Return if the action dictionary contains the key of name.
        /// </summary>
        public static bool IsActionRegistered(string name)
        {
            return actions.ContainsKey(name);
        }

        public static void EnableInput()
        {
            Map?.Enable();
        }

        public static void DisableInput()
        {
            Map?.Disable();
        }
    }
}
