using CardUITest.Cards.Hand;
using UnityEngine;

namespace CardUITest.Cards.DefaultImplementation
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(CardHighlighter))]
    public abstract class DefaultHeldableCard : TweenableCard, IHeldableCard
    {
        public CardHand CurrentHand { get; set; }

        private Transform _container;

        public Transform Container
        {
            get => _container;
            set
            {
                _container = value;
                if (value == null)
                {
                    _cardHighlighter.RemoveHighlight();
                }
            }
        }

        private CardHighlighter _cardHighlighter;

        protected virtual void Awake()
        {
            _cardHighlighter = GetComponent<CardHighlighter>();
        }

        private bool _isBusyByMe;

        /// <summary>
        /// This property helps identify whether card is busy by card itself and whether it should restore position or not
        /// </summary>
        private bool OwnBusy
        {
            get => IsBusy && _isBusyByMe;
            set
            {
                IsBusy = value;
                _isBusyByMe = value;
            }
        }

        private void OnMouseEnter()
        {
            if (Container is null || IsBusy) return;

            OwnBusy = true;
            transform.SetParent(null, true);
            _cardHighlighter.Highlight(Container);
        }

        private void OnMouseExit()
        {
            if (OwnBusy)
            {
                OwnBusy = false;
                transform.SetParent(Container, true);
                _cardHighlighter.RemoveHighlight(Container);
            }
        }
    }
}