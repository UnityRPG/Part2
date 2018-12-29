namespace RPG.Attributes
{
    [System.Serializable]
    public class AttributeModifier
    {
        public enum Attribute
        {
            DamageBonus,
            CriticalHitBonus,
            CriticalHitChance,
            HitSpeedBonus,
            Armour,
            ArmourBonus
        }

        public Attribute attribute;
        public float value;

        public AttributeModifier(Attribute attribute, float value)
        {
            this.attribute = attribute;
            this.value = value;
        }
    }
}
