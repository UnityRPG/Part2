using System.Collections.Generic;
using RPG.Progression;
using RPG.Stats;
using UnityEngine;

namespace RPG.Progression
{
    abstract public class CharacterLevel : MonoBehaviour, IStatModifiersProvider
    {
        [SerializeField] protected BaseStatsProgression statSet;
        public int level;

        abstract protected CoreBaseStats GetStats();

        public float GetBaseStat(string statId)
        {
            return health; //TODO: Fix;
        }

        IEnumerable<StatModifier> IStatModifiersProvider.GetModifiers(StatModifier.Filter filter)
        {
            var modifier = new StatModifier("health", health);
            if (modifier.Matches(filter))
            {
                yield return modifier;
            }
        }

        public float health
        {
            get
            {
                return GetStats().health[level];
            }
        }
    }
}
