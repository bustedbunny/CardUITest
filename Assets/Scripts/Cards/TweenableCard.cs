using DG.Tweening;
using UnityEngine;

namespace CardUITest.Cards
{
    public abstract class TweenableCard : MonoBehaviour, ITweenableCard
    {
        public bool IsBusy { get; set; }
        public Tween CurrentTween { get; set; }
        public Transform Transform => transform;


        protected virtual void OnDestroy()
        {
            CurrentTween?.Kill();
        }

        public abstract int GetValue(CardValueType type);
        public abstract void SetValue(CardValueType type, int value);
        public abstract void SetAsset(CardAsset asset);
    }
}