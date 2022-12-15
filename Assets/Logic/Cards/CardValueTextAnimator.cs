using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FMODUnity;
using TMPro;
using UnityEngine;

namespace CardUITest.Cards
{
    [RequireComponent(typeof(TextMeshPro))]
    public class CardValueTextAnimator : MonoBehaviour
    {
        [SerializeField] private float animationTime = 1f;
        private int _value;


        private StudioEventEmitter _audioEvent;

        private void Awake()
        {
            _text = GetComponent<TextMeshPro>();
            _audioEvent = GetComponent<StudioEventEmitter>();
        }

        private void OnDestroy()
        {
            Cancel();
        }

        private void Cancel()
        {
            if (_cts is not null)
            {
                _cts.Cancel();
                _cts.Dispose();
            }
        }

        private CancellationTokenSource _cts;

        public int Value
        {
            get => _value;
            set
            {
                Cancel();
                _cts = new CancellationTokenSource();
                Animate(value, _cts.Token).Forget();
            }
        }

        public int ValueNoAnimation
        {
            set
            {
                _shownValue = value;
                _text.text = _shownValue.ToString();
            }
        }

        private int _shownValue;
        private TextMeshPro _text;

        private async UniTaskVoid Animate(int value, CancellationToken ct)
        {
            var difference = animationTime / Mathf.Abs(_shownValue - value);

            while (_shownValue != value)
            {
                if (ct.IsCancellationRequested)
                {
                    return;
                }

                var step = DifferenceStep(_shownValue, value);
                _shownValue += step;
                _text.text = _shownValue.ToString();
                if (_audioEvent is not null)
                {
                    _audioEvent.Play();
                }

                await UniTask.Delay(TimeSpan.FromSeconds(difference), cancellationToken: ct);
            }
        }

        private static int DifferenceStep(int from, int to)
        {
            if (from > to)
            {
                return -1;
            }

            return to > from ? 1 : 0;
        }
    }
}