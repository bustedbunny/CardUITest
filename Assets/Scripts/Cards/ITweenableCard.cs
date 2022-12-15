using DG.Tweening;
using UnityEngine;

namespace CardUITest.Cards
{
    public interface ITweenableCard : ICard
    {
        
        public Tween CurrentTween { get; set; }
        public Transform Transform { get; }
    }
}