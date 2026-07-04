using UnityEngine;
using UnityEngine.InputSystem;

namespace Reign.Generic.UI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIElement : MonoBehaviour
    {
        public UIManager Manager {get; private set;}
        public string Key;
        public RectTransform rectTransform {get; private set;}

        private bool isSetUp = false;

        private void OnValidate()
        {
            // Editor only
            if (Application.isPlaying) return;

            if (string.IsNullOrEmpty(Key))
            {
                Debug.LogWarning($"Ensure the key of {this} is not empty. The UIManager will not be able to detect it.");
            }

            RefreshManager();
        }

        public abstract void TryTypeSearch();

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        protected virtual void Start()
        {
            if (!isSetUp)
            {
                Create(Key);
            }
        }

        protected virtual void OnDestroy()
        {
            Manager?.Unregister(this);
        }

        // API
        public void Create(string key)
        {
            Key = key;
            TryTypeSearch();
            RefreshManager();

            isSetUp = true;
        }

        public UIManager RefreshManager()
        {
            var found = GetComponentInParent<UIManager>();

            if (found == null) return null;

            if (Manager != found)
            {
                Manager?.Unregister(this);

                Manager = found;

                found.Register(this);
            }

            return found;
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