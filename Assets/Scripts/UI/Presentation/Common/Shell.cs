using UnityEngine;
using UnityEngine.UIElements;

namespace CardUITest.UI.Presentation.Common
{
    /// <summary>
    /// This class controls UI document and communications with Presentation objects
    /// </summary>
    public class Shell : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;

        [SerializeField] private PresentationBase startingPage;

        private VisualElement _root;

        private void Start()
        {
            _root = uiDocument.rootVisualElement;
            SetPage(startingPage);
        }

        public void SetPage(PresentationBase page)
        {
            var screen = page.Screen;
            if (_root.Contains(screen))
            {
                return;
            }

            _root.Clear();
            _root.Add(screen);
        }
    }
}