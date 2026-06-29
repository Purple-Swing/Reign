using TMPro;
using UnityEngine;

namespace Reign.Generic.UI.Subclasses
{
    [RequireComponent(typeof(TMP_Text))]
    public class UITextElement : UIElement
    {
        public TMP_Text textElement;

        public void SetText(string value)
        {
            textElement.text = value;
        }

        public void SetColor(Color color)
        {
            textElement.color = color;
        }

        public void SetFontStyle(FontStyles fontStyle)
        {
            textElement.fontStyle = fontStyle;
        }

        public void SetLetterSpacing(float spacing)
        {
            textElement.characterSpacing = spacing;
        }

        public void SetAlignment(TextAlignmentOptions alignment)
        {
            textElement.alignment = alignment;
        }

        public void SetFontAsset(TMP_FontAsset fontAsset)
        {
            textElement.font = fontAsset;
        }
    }
}