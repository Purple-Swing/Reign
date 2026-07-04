using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Reign.Generic.UI.Subclasses
{
    [RequireComponent(typeof(Image))]
    public class UIImageElement : UIElement
    {
        public Image imageElement;

        public override void TryTypeSearch()
        {
            imageElement = GetComponent<Image>();
        }

        public void SetColor(Color color)
        {
            imageElement.color = color;
        }

        public void SetSprite(Sprite sprite)
        {
            imageElement.sprite = sprite;
        }

        public void SetType(Image.Type type)
        {
            imageElement.type = type;
        }

        public void SetFillMethod(Image.FillMethod fillMethod)
        {
            imageElement.fillMethod = fillMethod;
        }

        public void SetFillAmount(float fillAmount)
        {
            imageElement.fillAmount = fillAmount;
        }

        public void SetFillClockwise(bool isClockwise)
        {
            imageElement.fillClockwise = isClockwise;
        }
    }
}