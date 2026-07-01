using Reign.Generic.UI;
using Reign.Generic.UI.Subclasses;
using UnityEngine;

namespace Reign.Internal
{
    public class Internal_UITest : MonoBehaviour
    {
        [SerializeField] private bool isEnabled;
        [SerializeField] private UIManager UI;

        private void Start()
        {
            if (!isEnabled) return;

            var copyrightText = UI.GetElement<UITextElement>("Text.CopyrightText");
            var logo = UI.GetElement<UIImageElement>("Image.Logo");
            var slider = UI.GetElement<UISliderElement>("Slider.Test");
            
            copyrightText.SetText("The UI Manager works!");
            slider.SetFillColor(Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f));
            logo.SetColor(Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f));
            copyrightText.SetColor(Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f));
        }
    }
}
