using CardUITest.Cards.Hand;
using DG.Tweening;
using UnityEngine;

namespace CardUITest.Cards.DefaultImplementation.Implementations
{
    /// <summary>
    /// Provides simple animation from card's current position into target position of deck
    /// </summary>
    [DisallowMultipleComponent]
    public class DefaultCardAnimator : MonoBehaviour, IDeckAnimationProvider
    {
        [SerializeField] private float duration = 1f;


        public void AnimateToPosition(ITweenableCard animated, Transform container)
        {
            var tran = animated.Transform;
            var sequence = DOTween.Sequence(tran);
            sequence.Insert(0f, tran.DORotateQuaternion(container.rotation, duration));
            sequence.Insert(0f, tran.DOMove(container.position, duration));

            animated.CurrentTween?.Kill();
            animated.CurrentTween = sequence;
        }
    }
}