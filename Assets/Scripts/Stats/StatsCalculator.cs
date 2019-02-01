using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class StatsCalculator : MonoBehaviour
    {
        IStatModifiersProvider[] modifierProviders;
        [SerializeField] float baseCriticalHit = 20;
        [SerializeField] float baseCriticalHitChance = 5;

        public float damageMultiplier
        {
            get
            {
                return (1 + damageBonus / 100f);
            }
        }

        public float damageBonus
        {
            get
            {
                return GetPercentageBonus("damage");
            }
        }

        public float criticalHitBonus
        {
            get
            {
                return baseCriticalHit + GetAdditiveTotal("criticalHitBonus");
            }
        }

        public float criticalHitChance
        {
            get
            {
                return baseCriticalHitChance + GetAdditiveTotal("criticalHitChance");
            }
        }

        public float totalDefence
        {
            get
            {
                return CalculateStat("armour");
            }
        }

        private void Start()
        {
            modifierProviders = GetComponents<IStatModifiersProvider>();
        }

        public float CalculateStat(string statId)
        {
            return (GetAdditiveTotal(statId)) * (1 + GetPercentageBonus(statId) / 100);
        }

        float GetAdditiveTotal(string statId)
        {
            float total = 0;
            foreach (var modifier in GetModifiersForStat(StatModifier.AdditiveFilter(statId)))
            {
                total += modifier.value;
            }
            return total;
        }

        float GetPercentageBonus(string statId)
        {
            float total = 0;
            foreach (var modifier in GetModifiersForStat(StatModifier.PercentageFilter(statId)))
            {
                total += modifier.value;
            }
            return total;
        }

        IEnumerable<StatModifier> GetModifiersForStat(StatModifier.Filter filter)
        {
            foreach (var modifierProvider in modifierProviders)
            {
                foreach (var modifier in modifierProvider.GetModifiers(filter))
                {
                    yield return modifier;
                }
            }
        }
    }
}