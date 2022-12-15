using System;
using TMPro;
using UnityEngine;

namespace CardUITest.Cards.DefaultImplementation
{
    [DisallowMultipleComponent]
    public class DefaultCardObject : DefaultHeldableCard
    {
        [SerializeField] private SpriteRenderer portraitRenderer;

        [SerializeField] private CardValueTextAnimator attackText;
        [SerializeField] private CardValueTextAnimator healthText;
        [SerializeField] private CardValueTextAnimator manaText;
        [SerializeField] private TextMeshPro titleText;
        [SerializeField] private TextMeshPro descriptionText;


        private int _health;
        private int _mana;
        private int _attack;


        public override int GetValue(CardValueType type) => type switch
        {
            CardValueType.Health => _health,
            CardValueType.Mana => _mana,
            CardValueType.Attack => _attack,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };


        public override void SetValue(CardValueType type, int value)
        {
            switch (type)
            {
                case CardValueType.Health:
                    _health = value;
                    UpdateText(healthText, value);
                    break;
                case CardValueType.Attack:
                    _attack = value;
                    UpdateText(attackText, value);
                    break;
                case CardValueType.Mana:
                    _mana = value;
                    UpdateText(manaText, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static void UpdateText(CardValueTextAnimator text, int value) => text.Value = value;

        public override void SetAsset(CardAsset source)
        {
            portraitRenderer.sprite = source.portrait;

            _health = source.health;
            healthText.ValueNoAnimation = source.health;
            _attack = source.attack;
            attackText.ValueNoAnimation = source.attack;
            _mana = source.mana;
            manaText.ValueNoAnimation = source.mana;

            titleText.text = source.title;
            descriptionText.text = source.description;
        }
    }
}