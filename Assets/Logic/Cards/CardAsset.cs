using UnityEngine;

namespace CardUITest.Cards
{
    [CreateAssetMenu(fileName = "CardAsset", menuName = "ScriptableObjects/CardAsset", order = 1)]
    public class CardAsset : ScriptableObject
    {
        public Sprite portrait;

        public string title;
        public string description;

        public int health;
        public int attack;
        public int mana;
    }
}