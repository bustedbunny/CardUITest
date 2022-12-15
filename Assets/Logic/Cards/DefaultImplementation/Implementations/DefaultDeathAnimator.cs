using Cysharp.Threading.Tasks;
using DG.Tweening;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardUITest.Cards.DefaultImplementation.Implementations
{
    public class DefaultDeathAnimator : MonoBehaviour, ICardDeathAnimationProvider
    {
        [SerializeField] private float shakeDuration;

        [SerializeField] private Vector3 positionShakeStrength = Vector3.one;
        [SerializeField] private Vector3 scaleShakeStrength = Vector3.one;

        [SerializeField] private StudioEventEmitter deathAudio;

        public void DestroyCard(IHeldableCard card)
        {
            card.CurrentTween?.Kill();
            card.IsBusy = true;
            card?.CurrentHand.RemoveCard(card);

            var tran = card.Transform;

            tran.SetParent(null, true);

            // First we put card on the front
            var targetPosition = tran.position;
            targetPosition.z -= 5f;
            tran.position = targetPosition;


            var seq = DOTween.Sequence(card);
            seq.Insert(0f, tran.DOShakePosition(shakeDuration, positionShakeStrength));
            seq.Insert(0f, tran.DOShakeScale(shakeDuration, scaleShakeStrength));

            seq.onComplete += () =>
            {
                deathAudio.Play();
                Destroy(card.Transform.gameObject);
            };


            card.CurrentTween = seq;
        }
    }
}