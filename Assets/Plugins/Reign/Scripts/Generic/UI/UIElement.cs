using UnityEngine;

namespace Reign.Generic.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        public UIManager Manager {get; private set;}
        public string Key;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(Key))
            {
                Debug.LogWarning("Key cannot be validated if null or empty.");
                return;
            }
            
            var foundManager = GetComponentInParent<UIManager>();
            if (Manager != foundManager)
            {
                Manager?.Unregister(this);

                Manager = foundManager;

                // Ensure the correct order by using foundManager, even after setting the current manager.
                foundManager?.Register(this);
            }
        }

        protected virtual void Awake()
        {
            Manager?.Register(this);
        }

        protected virtual void OnDestroy()
        {
            Manager?.Unregister(this);
        }

        public void SetActive(bool active)
        {
            // This is the only method inherited by all, because every element should have a game object.
            
            // Also slightly simplifies the verbose nature of a line like:
            // uiManager.GetElement<UITextElement>("MyText").gameObject.SetActive(false)

            gameObject.SetActive(active);
        }
    }
}