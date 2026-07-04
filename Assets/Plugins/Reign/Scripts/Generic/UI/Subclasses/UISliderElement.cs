using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Reign.Generic.UI.Subclasses
{
    [RequireComponent(typeof(Slider))]
    public class UISliderElement : UIElement
    {
        public Slider sliderElement;
        [SerializeField] private Image backgroundImage;
        private Image fillImage;

        public override void TryTypeSearch()
        {
            sliderElement = GetComponent<Slider>();
        }

        private void OnValidate()
        {
            if (sliderElement == null) return;

            // Try find images
            fillImage ??= sliderElement.fillRect.GetComponent<Image>();
            backgroundImage ??= sliderElement.transform.Find("Background").GetComponent<Image>();
        }

        public void SetValue(float value)
        {
            sliderElement.value = value;
        }

        public void SetHandleColor(Color color)
        {
            sliderElement.image.color = color;
        } 
        
        public void SetBackgroundColor(Color color)
        {
            if (backgroundImage == null)
            {
                Debug.LogWarning($"Background image of slider on {gameObject.name} not assigned.");
                return;
            }

            backgroundImage.color = color;
        }
        
        public void SetFillColor(Color color)
        {
            fillImage ??= sliderElement.fillRect.GetComponent<Image>(); 
            
            if (fillImage == null)
            { 
                Debug.LogWarning($"Fill image of slider on {gameObject.name} not found.");
                return;
            }

            fillImage.color = color;
        }

        public void SetValueBoundaries(float min, float max)
        {
            sliderElement.maxValue = max;
            sliderElement.minValue = min;
        
            sliderElement.value = Mathf.Clamp(sliderElement.value, sliderElement.minValue, sliderElement.maxValue);
        }

        public void SetBackgroundImageType(Image.Type type, float ppuMultiplier)
        {
            if (backgroundImage == null)
            {
                Debug.LogWarning($"Background image of slider on {gameObject.name} not assigned.");
                return;
            }

            backgroundImage.type = type;
            backgroundImage.pixelsPerUnitMultiplier = ppuMultiplier;
        }

        public void SetHandleImageType(Image.Type type, float ppuMultiplier)
        {
            sliderElement.image.type = type;
            sliderElement.image.pixelsPerUnitMultiplier = ppuMultiplier;
        }

        public void SetFillImageType(Image.Type type, float ppuMultiplier)
        {
            fillImage ??= sliderElement.fillRect.GetComponent<Image>(); 

            if (fillImage == null)
            { 
                Debug.LogWarning($"Fill image of slider on {gameObject.name} not found.");
                return;
            }

            fillImage.type = type;
            fillImage.pixelsPerUnitMultiplier = ppuMultiplier;
        }

        public void SetHandleSprite(Sprite sprite)
        {
            sliderElement.image.sprite = sprite;
        }
        
        public void SetBackgroundSprite(Sprite sprite)
        {
            if (backgroundImage == null)
            {
                Debug.LogWarning($"Background image of slider on {gameObject.name} not assigned.");
                return;
            }

            backgroundImage.sprite = sprite;
        }

        public void SetFillSprite(Sprite sprite)
        {
            fillImage ??= sliderElement.fillRect.GetComponent<Image>(); 

            if (fillImage == null)
            { 
                Debug.LogWarning($"Fill image of slider on {gameObject.name} not found.");
                return;
            }

            fillImage.sprite = sprite;
        }
    }
}