﻿using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

namespace RPG.Progression
{
    abstract public class CharacterLevel : MonoBehaviour, IStatModifiersProvider
    {
        [SerializeField] protected BaseStatsProgression statSet;
        public int level;

        abstract protected CoreBaseStats GetStats();

        IEnumerable<StatModifier> IStatModifiersProvider.GetModifiers(StatModifier.Filter filter)
        {
            var modifier = new StatModifier(StatId.Health, health);
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
