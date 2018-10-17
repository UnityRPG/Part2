namespace RPG.Characters
{
    [System.Serializable]
    public class StatsModifier
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
    }
}
