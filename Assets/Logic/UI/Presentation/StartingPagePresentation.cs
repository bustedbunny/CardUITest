using CardUITest.UI.Presentation.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardUITest.UI.Presentation
{
    public class StartingPagePresentation : PresentationBase
    {
        [SerializeField] private GameInstance gameInstance;
        protected override void Awake()
        {
            base.Awake();

            var startButton = Screen.Q<Button>("StartButton");
            startButton.RegisterCallback<ClickEvent>(_ => { gameInstance.StartTheGame(); });
        }
    }
}