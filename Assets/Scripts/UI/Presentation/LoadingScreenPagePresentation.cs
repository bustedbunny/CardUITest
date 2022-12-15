using System;
using CardUITest.UI.Presentation.Common;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace CardUITest.UI.Presentation
{
    public class LoadingScreenPagePresentation : PresentationBase
    {
        private ProgressBar _progressBar;
        private IProgress<float> _progress;
        private float _maxValue;

        protected override void Awake()
        {
            base.Awake();

            _progressBar = Screen.Q<ProgressBar>();
            _progressBar.lowValue = 0f;
            _progressBar.highValue = 1f;

            _progress = Progress.Create<float>(UpdateBar);
        }

        private void UpdateBar(float value)
        {
            _progressBar.value = value;
        }

        /// <summary>
        /// Opens loading screen page with progress bar
        /// </summary>
        /// <returns>ProgressBar that accepts normalized (from 0 to 1) float values</returns>
        public IProgress<float> StartLoadingScreenWithProgressBar()
        {
            Shell.SetPage(this);
            return _progress;
        }
    }
}