using UnityEngine;

namespace Reign.Generic.UI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIElement : MonoBehaviour
    {
        public UIManager Manager {get; private set;}
        public string Key;
        public RectTransform rectTransform {get; private set;}

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(Key))
            {
                Debug.LogWarning($"Ensure the key of {this} is not empty. The UIManager will not be able to detect it.");
            }

            var found = GetComponentInParent<UIManager>();

            if (found == null) return;

            if (Manager != found)
            {
                Manager?.Unregister(this);

                Manager = found;
    
                found.Register(this);
            }
        }

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            Manager?.Register(this);
        }

        protected virtual void OnDestroy()
        {
            Manager?.Unregister(this);
        }

        // These are the methods inherited by all, because every element should have a game object.
            
        // These also slightly simplify the verbose nature of a line like:
        // uiManager.GetElement<UITextElement>("MyText").gameObject.SetActive(false)

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void SetRotation(Vector3 rotation)
        {
            rectTransform.eulerAngles = rotation;
        }

        public void SetAnchoredPosition(Vector2 anchored)
        {
            rectTransform.anchoredPosition = anchored;
        }

        public void SetSize(Vector2 size)
        {
            rectTransform.sizeDelta = size;
        }

        public void SetPivot(Vector2 pivotPoint)
        {
            rectTransform.pivot = pivotPoint;
        }
    }
}