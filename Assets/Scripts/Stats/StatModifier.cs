namespace RPG.Stats
{
    [System.Serializable]
    public class StatModifier
    {
        public FinalStat stat;
        public enum AggregationType
        {
            Additive,
            PercentageBonus
        }

        public string statId;
        public float value;
        public AggregationType aggregationType;

        public StatModifier(string statId, float value) : this(statId, value, AggregationType.Additive)
        {
        }

        public StatModifier(string statId, float value, AggregationType aggregationType)
        {
            this.statId = statId;
            this.value = value;
            this.aggregationType = aggregationType;
        }
    }
}
