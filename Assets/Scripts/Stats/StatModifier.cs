using System.Collections.Generic;

namespace RPG.Stats
{
    [System.Serializable]
    public class StatModifier
    {
        public class Filter
        {
            public string statId;
            public AggregationType aggregationType;
            public Filter(string statId, AggregationType aggregationType)
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

        public static Filter PercentageFilter(string statId)
        {
            return new Filter(statId, AggregationType.PercentageBonus);
        }

        public static Filter AdditiveFilter(string statId)
        {
            return new Filter(statId, AggregationType.Additive);
        }

        public bool Matches(Filter filter)
        {
            return statId == filter.statId && aggregationType == filter.aggregationType;
        }
    }
}
