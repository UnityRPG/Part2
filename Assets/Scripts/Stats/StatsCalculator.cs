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
                return GetPercentageBonus(StatId.Damage);
            }
        }

        public float criticalHitBonus
        {
            get
            {
                return baseCriticalHit + GetAdditiveTotal(StatId.CriticalHitBonus);
            }
        }

        public float criticalHitChance
        {
            get
            {
                return baseCriticalHitChance + GetAdditiveTotal(StatId.CriticalHitChance);
            }
        }

        public float totalDefence
        {
            get
            {
                return CalculateStat(StatId.Armour);
            }
        }

        private void Start()
        {
            modifierProviders = GetComponents<IStatModifiersProvider>();
        }

        public float CalculateStat(StatId statId)
        {
            return (GetAdditiveTotal(statId)) * (1 + GetPercentageBonus(statId) / 100);
        }

        float GetAdditiveTotal(StatId statId)
        {
            float total = 0;
            foreach (var modifier in GetModifiersForStat(StatModifier.AdditiveFilter(statId)))
            {
                total += modifier.value;
            }
            return total;
        }

        float GetPercentageBonus(StatId statId)
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