namespace RPG.Attributes
{
    [System.Serializable]
    public class PerformanceModifier
    {
        public enum PerformanceStat
        {
            DamageBonus,
            CriticalHitBonus,
            CriticalHitChance,
            HitSpeedBonus,
            Armour,
            ArmourBonus
        }

        public PerformanceStat stat;
        public float value;

        public PerformanceModifier(PerformanceStat stat, float value)
        {
            this.stat = stat;
            this.value = value;
        }
    }
}
