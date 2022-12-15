using CardUITest.GamePlay;
using CardUITest.UI.Presentation.Common;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardUITest.UI.Presentation
{
    public class PlayPagePresentation : PresentationBase
    {
        [SerializeField] private GameInstance gameInstance;

        [SerializeField] private CardValueRandomizer randomizer;

        protected override void Awake()
        {
            base.Awake();

            var pauseButton = Screen.Q<Button>("PauseButton");
            pauseButton.RegisterCallback<ClickEvent>(_ => { gameInstance.StopTheGame(); });


            _randomChangeButton = Screen.Q<Button>("RandomChangeButton");
            _initialText = _randomChangeButton.text;
            _randomChangeButton.RegisterCallback<ClickEvent>(_ => { OnRandomChange().Forget(); });
        }

        private string _initialText;
        private const string BusyText = "Busy";


        private Button _randomChangeButton;

        private bool isPlaying;

        private async UniTask OnRandomChange()
        {
            if (isPlaying)
            {
                return;
            }

            isPlaying = true;

            _randomChangeButton.text = BusyText;

            await randomizer.ChangeValueRandomly().SuppressCancellationThrow();

            _randomChangeButton.text = _initialText;

            isPlaying = false;
        }
    }
}