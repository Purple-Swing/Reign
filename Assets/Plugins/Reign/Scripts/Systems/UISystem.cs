using Reign.Generic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Reign.Systems
{
    public sealed class UISystem : System<UISystem>
    {
        public UIManager CreateUIManager(out Canvas managerCanvas, Vector2 scalerReferenceResolution, CanvasScaler.ScaleMode scalerScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize, CanvasScaler.ScreenMatchMode scalerMatchMode = CanvasScaler.ScreenMatchMode.Expand, Transform parent = null)
        {
            // Create new UI Manager object
            GameObject manager = new("UI Manager");

            // Set parent
            manager.transform.SetParent(parent, false);

            // Create Canvas component
            var canvas = manager.AddComponent<Canvas>();

            // Set render mode
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // Create CanvasScaler component
            var scaler = manager.AddComponent<CanvasScaler>();

            // Set reference resolution and scale mode to parameters
            scaler.referenceResolution = scalerReferenceResolution;
            scaler.uiScaleMode = scalerScaleMode;
            scaler.screenMatchMode = scalerMatchMode;

            // Add UIManager component
            UIManager component = manager.AddComponent<UIManager>();

            // Out manager canvas
            managerCanvas = canvas;

            // Return UIManager component
            return component;
        }
        
        public T CreateUIElement<T>(string key, Canvas canvas) where T : UIElement
        {
            // Create new game object with name of the key
            GameObject element = new(key);

            // Set parent to canvas
            element.transform.parent = canvas.transform;

            // Add UIElement to new game object
            T component = element.AddComponent<T>();

            // Initialise component
            component.Create(key);

            // Validate manager
            component.RefreshManager();

            return component;
        }

        public UIElement CreateUIElementPrefab(GameObject prefab, Canvas canvas)
        {
            // Create new element based on prefab as child of canvas
            GameObject element = Instantiate(prefab, canvas.transform);

            // Update active manager
            if (element.TryGetComponent<UIElement>(out var component))
            {
                component.RefreshManager();
                return component;
            }
            
            Debug.LogError($"No UIElement component found on prefab of {element.name}");
            return null;
        }
    }
}