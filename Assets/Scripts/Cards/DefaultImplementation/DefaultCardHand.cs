using System.Collections.Generic;
using System.Linq;
using CardUITest.Cards.Hand;
using UnityEngine;
using UnityEngine.Assertions;

namespace CardUITest.Cards.DefaultImplementation
{
    [RequireComponent(typeof(IDeckAnimationProvider))]
    [RequireComponent(typeof(IHandPositionProvider))]
    public class DefaultCardHand : CardHand
    {
        private IDeckAnimationProvider _cardAnimator;
        private IHandPositionProvider _positionProvider;

        private void Awake()
        {
            _cardAnimator = GetComponent<IDeckAnimationProvider>();
            _positionProvider = GetComponent<IHandPositionProvider>();
        }

        private readonly List<IHeldableCard> _records = new();


        private int _currentIndex;

        public override IHeldableCard GetNextCard
        {
            get
            {
                if (_records.Count <= _currentIndex)
                {
                    _currentIndex = 0;
                }

                var cur = _records[_currentIndex];
                _currentIndex++;
                return cur;
            }
        }

        public override void AddCard(IHeldableCard card)
        {
            Assert.IsFalse(_records.Any(x => x == card));

            var container = _positionProvider.CreatePosition();

            card.Container = container;
            card.CurrentHand = this;
            _records.Add(card);

            UpdatePositionsAndAnimate();
        }

        public override void RemoveCard(IHeldableCard card)
        {
            Assert.IsTrue(_records.Any(x => x == card));

            // This little manipulation assures our real next card is not skipped
            var ind = _records.IndexOf(card);
            if (_currentIndex > ind) _currentIndex = ind;
            _records.RemoveAt(ind);

            card.CurrentHand = null;
            card.Transform.SetParent(null, true);
            _positionProvider.RemovePosition(card.Container);

            UpdatePositionsAndAnimate();
        }

        public override void RestorePosition(IHeldableCard card)
        {
            Assert.IsTrue(_records.Any(x => x == card));

            AnimateCardToPosition(card);
        }

        private void AnimateCardToPosition(IHeldableCard card)
        {
            card.Transform.SetParent(card.Container, true);

            _cardAnimator.AnimateToPosition(card, card.Container);
        }

        private void UpdatePositionsAndAnimate()
        {
            // Temporary unparent all held cards, so they can get animation to their new positions
            foreach (var heldCard in _records)
            {
                if (heldCard.IsBusy)
                {
                    continue;
                }

                heldCard.Transform.SetParent(null, true);
            }

            _positionProvider.UpdatePositions();

            foreach (var heldCard in _records)
            {
                if (heldCard.IsBusy)
                {
                    continue;
                }

                AnimateCardToPosition(heldCard);
            }
        }
    }
}