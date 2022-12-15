using UnityEngine;

namespace CardUITest.Cards.Hand
{
    public interface IHandPositionProvider
    {
        public Transform CreatePosition();
        public void RemovePosition(Transform position);
        public void UpdatePositions();
    }
}