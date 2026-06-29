using Reign.Generic.UI;
using Reign.Generic.UI.Subclasses;
using UnityEngine;

namespace Reign.Internal
{
    public class Internal_UITest : MonoBehaviour
    {
        [SerializeField] private UIManager UI;
        private UITextElement copyrightText;

        private void Start()
        {
            copyrightText = UI.GetElement<UITextElement>("Text.CopyrightText");
            
            copyrightText.SetText("The UI Manager works!");
            copyrightText.SetColor(Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f));
        }
    }
}
