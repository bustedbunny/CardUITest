using UnityEngine;
using UnityEngine.UIElements;

namespace CardUITest.UI.Presentation.Common
{
    /// <summary>
    /// Base class to provide functionality for UXML documents within Shell controlled UIDocument
    /// </summary>
    [RequireComponent(typeof(Shell))]
    public abstract class PresentationBase : MonoBehaviour
    {
        protected Shell Shell;

        [SerializeField] private VisualTreeAsset screenAsset;
        public VisualElement Screen { get; private set; }

        protected virtual void Awake()
        {
            Shell = GetComponent<Shell>();
            Screen = screenAsset.Instantiate();

            // This is required so VisualElements as they get attached to root VE, will fill whole screen
            Screen.style.flexGrow = new StyleFloat(1f);
        }
    }
}