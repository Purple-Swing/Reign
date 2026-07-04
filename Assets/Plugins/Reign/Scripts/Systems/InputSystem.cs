using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Reign.Enums;
using UnityEngine.InputSystem.Controls;
using System.Collections.Generic;
using Reign.Events;

namespace Reign.Systems
{
    public struct OnInputEvent : IEvent 
    {
        public InputAction inputAction;
        public ButtonControl buttonControl;
        public InputType type;
        public InputActionMap currentActionMap;
    }

    public class InputSystem : System<InputSystem>
    {
        private InputActionMap currentMap;
        private readonly Dictionary<string, InputAction> actionsCached = new();
        [Min(0), SerializeField] private int ActiveActionMapIndex = 0;
        [SerializeField] private InputActionAsset inputActions;

        private void Awake()
        {
            Validate();
        }

        public void UpdateActiveMapIndex(int newIndex)
        {
            ActiveActionMapIndex = newIndex;
            Validate();
        }

        private void Validate()
        {
            if (inputActions == null)
            {
                throw new Exception("Input actions must be assigned.");
            }

            if (ActiveActionMapIndex < 0 || ActiveActionMapIndex >= inputActions.actionMaps.Count)
            {
                throw new Exception("Action map index must be valid (greater than or equal to 0).");
            }

            currentMap = inputActions.actionMaps[ActiveActionMapIndex];
            currentMap.Enable();

            RebuildCache();
        }

        private void RebuildCache()
        {
            actionsCached.Clear();

            foreach (var action in currentMap.actions)
            {
                if (!actionsCached.TryAdd(action.name, action))
                {
                    Debug.LogWarning($"Duplicate action name @ {action.name}");
                }
            }
        }

        /// <summary>
        /// Find action on the current input map by name
        /// </summary>
        public InputAction FindAction(string name)
        {
            if (actionsCached.TryGetValue(name, out var action))
            {
                return action;
            }

            Debug.LogWarning($"Action '{name}' could not be found, was it cached correctly?");
            return null;
        }

        /// <summary>
        /// Read value from action by name
        /// </summary>
        public T GetValue<T>(string actionName) where T : struct
        {
            if (currentMap == null)
            {
                Debug.LogWarning("No active action map.");
                return default;
            }

            var action = FindAction(actionName);

            if (action == null)
            {
                Debug.LogWarning($"Action '{actionName}' not found.");
                return default;
            }

            return action.ReadValue<T>();
        }

        private bool IsValid(InputType inputType, ButtonControl action)
        {
            _ = EventBus.Publish(new OnInputEvent { currentActionMap = currentMap, buttonControl = action, inputAction = null, type = inputType });
            return inputType switch
            {
                InputType.DEFERRED => throw new Exception("DEFERRED input types cannot evaluate ButtonControl."),
                InputType.PRESSED => action.wasPressedThisFrame,
                InputType.RELEASED => action.wasReleasedThisFrame,
                InputType.PRESSING => action.isPressed,
                _ => false
            };
        }

        private bool IsValid(InputType inputType, InputAction action)
        {
            _ = EventBus.Publish(new OnInputEvent { currentActionMap = currentMap, buttonControl = null, inputAction = action, type = inputType });
            return inputType switch
            {
                InputType.DEFERRED => action.WasPerformedThisFrame(),
                InputType.PRESSED => action.WasPressedThisFrame(),
                InputType.RELEASED => action.WasReleasedThisFrame(),
                InputType.PRESSING => action.IsPressed(),
                _ => false
            };
        }

        /// <summary>
        /// Get button down by action name and input type.
        /// </summary>
        public bool GetButton(string actionName, InputType inputType = InputType.DEFERRED, bool requireAllBindings = false)
        {
            if (currentMap == null) return false;

            var action = FindAction(actionName);
            if (action == null) return false;

            // Only one needed to be down
            if (!requireAllBindings || inputType == InputType.DEFERRED)
            {
                return IsValid(inputType, action);
            }

            // Every one must be down
            foreach (var control in action.controls)
            {
                if (control is ButtonControl button)
                {
                    if (!IsValid(inputType, button)) return false;
                }
            }

            return true;
        }
    }
}
