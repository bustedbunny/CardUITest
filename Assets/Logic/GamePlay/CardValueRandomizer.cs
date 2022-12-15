using System;
using System.Threading;
using CardUITest.Cards;
using CardUITest.Cards.DefaultImplementation.Implementations;
using CardUITest.Cards.Hand;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardUITest.GamePlay
{
    public class CardValueRandomizer : MonoBehaviour
    {
        [SerializeField] private DefaultDeathAnimator deathAnimator;
        [SerializeField] private CardHand cardHand;


        [SerializeField] private float cardFlyDuration;

        [SerializeField] private float textUpdateAwaitDuration = 1f;

        private static CardValueType[] _types = (CardValueType[])Enum.GetValues(typeof(CardValueType));

        private CancellationToken _ct;

        private void Awake()
        {
            _ct = this.GetCancellationTokenOnDestroy();
        }

        public async UniTask ChangeValueRandomly()
        {
            var card = cardHand.GetNextCard;

            // This token helps prevent errors if object was destroyed (for example, game was stopped) mid animation
            var ct = CancellationTokenSource.CreateLinkedTokenSource(_ct,
                card.Transform.GetCancellationTokenOnDestroy()).Token;


            if (card.IsBusy)
            {
                return;
            }

            card.IsBusy = true;

            card.CurrentTween?.Kill();

            var tran = card.Transform;
            tran.SetParent(null, true);


            var sequence = DOTween.Sequence(card);
            sequence.Insert(0f, tran.DOMove(transform.position, cardFlyDuration));
            sequence.Insert(0f, tran.DORotateQuaternion(Quaternion.identity, cardFlyDuration));

            card.CurrentTween = sequence;

            var isDone = false;
            sequence.onComplete += () => isDone = true;

            await UniTask.WaitUntil(() => isDone, cancellationToken: ct);

            card.SetValue(_types[Random.Range(0, _types.Length)], Random.Range(-2, 9));

            await UniTask.Delay(TimeSpan.FromSeconds(textUpdateAwaitDuration), cancellationToken: ct);

            if (card.GetValue(CardValueType.Health) <= 0)
            {
                deathAnimator.DestroyCard(card);
                return;
            }

            card.IsBusy = false;
            cardHand.RestorePosition(card);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}