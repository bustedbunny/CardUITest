using UnityEngine;

namespace CardUITest.Cards.Hand
{
    /// <summary>
    /// We use MonoBehaviour as base instead of interface, so other monobs can serialize it
    /// </summary>
    public abstract class CardHand : MonoBehaviour
    {
        public abstract IHeldableCard GetNextCard { get; }
        public abstract void AddCard(IHeldableCard card);
        public abstract void RemoveCard(IHeldableCard card);
        public abstract void RestorePosition(IHeldableCard card);
    }
}