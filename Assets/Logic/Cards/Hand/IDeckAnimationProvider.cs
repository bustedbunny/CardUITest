using UnityEngine;

namespace CardUITest.Cards.Hand
{
    public interface IDeckAnimationProvider
    {
        public void AnimateToPosition(ITweenableCard animated, Transform container);
    }
}