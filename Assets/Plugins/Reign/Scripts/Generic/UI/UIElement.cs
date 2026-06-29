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
                Debug.LogWarning($"Ensure the key of {this} is not empty. The UIManager will not be able to detect it.");
            }

            var attempt = GetComponentInParent<UIManager>();

            if (attempt != null)
            {
                // Only update given that we NEED to.
                if (Manager != attempt) Manager = attempt;
            }
            else
            {
                Debug.LogWarning($"UIManager in parent object of {gameObject.name} could not be found.");
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