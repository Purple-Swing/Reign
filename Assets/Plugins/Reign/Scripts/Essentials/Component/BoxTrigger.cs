using NaughtyAttributes;
using Reign.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Reign.Essentials
{
    [RequireComponent(typeof(BoxCollider))]
    public class BoxTrigger : MonoBehaviour
    {
        public BoxCollider ColliderReference { get; private set; }
        public Collider LastInteracted { get; private set; }
        public bool triggerEnabled = true;

        [SerializeField, Tag] string searchTag;
        public event Action OnTriggerEntered;
        public event Action OnTriggerExited;

        private void Awake()
        {
            ColliderReference = GetComponent<BoxCollider>();

            if (ColliderReference != null) ColliderReference.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (triggerEnabled && (other.CompareTag(searchTag) || string.IsNullOrEmpty(searchTag)))
            {
                LastInteracted = other;
                OnTriggerEntered?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (triggerEnabled && (other.CompareTag(searchTag) || string.IsNullOrEmpty(searchTag)))
            {
                LastInteracted = other;
                OnTriggerExited?.Invoke();
            }
        }

        private void OnDestroy()
        {
            OnTriggerExited?.Invoke();
        }

        public void EnableTrigger() => triggerEnabled = true;
        public void DisableTrigger() => triggerEnabled = false;
        public void ToggleTrigger() => triggerEnabled = !triggerEnabled;
    }
}
