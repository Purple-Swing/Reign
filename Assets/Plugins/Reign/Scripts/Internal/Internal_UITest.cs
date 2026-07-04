using Reign.Generic.UI;
using Reign.Generic.UI.Subclasses;
using Reign.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Reign.Internal
{
    public class Internal_UITest : MonoBehaviour
    {
        [SerializeField] private bool isEnabled;

        private void Start()
        {
            if (!isEnabled) return;

            var uiMan = UISystem.Instance.CreateUIManager(out Canvas canvas, new(1280, 720));

            var newElement = UISystem.Instance.CreateUIElement<UITextElement>("NewUIElement", canvas);
            newElement.SetText("Hello! I am created via script...");
            newElement.SetAlignment(TMPro.TextAlignmentOptions.Center);
            newElement.SetSize(new(500, 290));
            newElement.SetColor(Color.white);
            newElement.SetPivot(new(0.5f, 0.5f));
            newElement.SetAnchoredPosition(new(0, 0));
        }
    }
}
