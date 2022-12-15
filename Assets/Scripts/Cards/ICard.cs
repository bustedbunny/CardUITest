namespace CardUITest.Cards
{
    public interface ICard
    {
        public int GetValue(CardValueType type);
        public void SetValue(CardValueType type, int value);

        public void SetAsset(CardAsset asset);
    }

    public enum CardValueType
    {
        Health,
        Attack,
        Mana
    }
}