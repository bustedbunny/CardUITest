using CardUITest.Cards.Hand;
using UnityEngine;

namespace CardUITest.Cards
{
    public interface IHeldableCard : ITweenableCard
    {
        public bool IsBusy { get; set; }
        public CardHand CurrentHand { get; set; }
        public Transform Container { get; set; }
    }
}