using System.Collections.Generic;

namespace RPG.Stats
{
    [System.Serializable]
    public class StatModifier
    {
        public class Filter
        {
            public StatId statId;
            public AggregationType aggregationType;
            public Filter(StatId statId, AggregationType aggregationType)
            {
                this.statId = statId;
                this.aggregationType = aggregationType;
            }
        }

        public enum AggregationType
        {
            Additive,
            PercentageBonus
        }

        public StatId statId;
        public float value;
        public AggregationType aggregationType;

        public StatModifier(StatId statId, float value) : this(statId, value, AggregationType.Additive)
        {
        }

        public StatModifier(StatId statId, float value, AggregationType aggregationType)
        {
            this.statId = statId;
            this.value = value;
            this.aggregationType = aggregationType;
        }

        public static Filter PercentageFilter(StatId statId)
        {
            return new Filter(statId, AggregationType.PercentageBonus);
        }

        public static Filter AdditiveFilter(StatId statId)
        {
            return new Filter(statId, AggregationType.Additive);
        }

        public bool Matches(Filter filter)
        {
            return statId == filter.statId && aggregationType == filter.aggregationType;
        }
    }
}
