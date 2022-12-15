using DG.Tweening;
using FMODUnity;
using UnityEngine;

namespace CardUITest.Cards.DefaultImplementation
{
    [RequireComponent(typeof(ITweenableCard))]
    public class CardHighlighter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer highlight;
        [SerializeField] private float animationDuration = 0.5f;
        [SerializeField] private float zOffset = -1f;
        [SerializeField] private float yOffset = 1f;

        private ITweenableCard _tweenable;

        private StudioEventEmitter _audioEvent;

        private void Awake()
        {
            _tweenable = GetComponent<ITweenableCard>();
            _audioEvent = GetComponent<StudioEventEmitter>();
            RemoveAlpha();
        }

        private void RemoveAlpha()
        {
            if (highlight is not null)
            {
                var highlightColor = highlight.color;
                highlightColor.a = 0f;
                highlight.color = highlightColor;
            }
        }

        public void Highlight(Transform container = null)
        {
            _tweenable.CurrentTween?.Kill();

            var tran = transform;
            var sequence = DOTween.Sequence(tran);
            if (container is not null)
            {
                sequence.Insert(0f, tran.DOMove(TargetMovePosition(container), animationDuration));
            }

            sequence.Insert(0f, tran.DOScale(1.25f, animationDuration));
            sequence.Insert(0f, highlight.DOFade(1f, animationDuration));


            var shouldCleanup = true;

            sequence.onComplete += () => shouldCleanup = false;
            sequence.onKill += () =>
            {
                if (shouldCleanup)
                {
                    RemoveAlpha();
                }
            };
            _tweenable.CurrentTween = sequence;

            if (_audioEvent is not null)
            {
                _audioEvent.Play();
            }
        }

        public void RemoveHighlight(Transform container = null)
        {
            _tweenable.CurrentTween?.Kill();


            var tran = transform;
            var sequence = DOTween.Sequence(tran);

            if (container is not null)
            {
                sequence.Insert(0f, tran.DOMove(container.position, animationDuration));
            }

            sequence.Insert(0f, tran.DOScale(1f, animationDuration));
            sequence.Insert(0f, highlight.DOFade(0f, animationDuration));


            // It's important to restore alpha and scale, so we do this callback to ensure it
            sequence.OnKill(() =>
            {
                tran.localScale = Vector3.one;
                RemoveAlpha();
            });

            _tweenable.CurrentTween = sequence;
        }

        private Vector3 TargetMovePosition(Transform container)
        {
            var targetPosition = container.position;
            targetPosition.z += zOffset;
            targetPosition.y += yOffset;
            return targetPosition;
        }
    }
}