using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class PerformanceStatCalculator : MonoBehaviour
    {
        IPerformanceModifierProvider[] modifierProviders;
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
                return SumModifiersForAttribute(PerformanceModifier.PerformanceStat.DamageBonus);
            }
        }

        public float criticalHitBonus
        {
            get
            {
                return baseCriticalHit + SumModifiersForAttribute(PerformanceModifier.PerformanceStat.CriticalHitBonus);
            }
        }

        public float criticalHitChance
        {
            get
            {
                return baseCriticalHitChance + SumModifiersForAttribute(PerformanceModifier.PerformanceStat.CriticalHitChance);
            }
        }

        public float armour
        {
            get
            {
                return SumModifiersForAttribute(PerformanceModifier.PerformanceStat.Armour);
            }
        }

        public float armourBonus
        {
            get
            {
                return SumModifiersForAttribute(PerformanceModifier.PerformanceStat.Armour);
            }
        }

        public float totalDefence
        {
            get
            {
                return armour * (1 + armourBonus / 100);
            }
        }

        private void Start()
        {
            modifierProviders = GetComponents<IPerformanceModifierProvider>();
        }

        float SumModifiersForAttribute(PerformanceModifier.PerformanceStat attribute)
        {
            float total = 0;
            foreach (var modifier in GetAttributeModifiersForAttribute(attribute))
            {
                total += modifier.value;
            }
            return total;
        }

        IEnumerable<PerformanceModifier> GetAttributeModifiersForAttribute(PerformanceModifier.PerformanceStat attribute)
        {
            foreach (var modifierProvider in modifierProviders)
            {
                foreach (var modifier in modifierProvider.modifiers)
                {
                    if (modifier.stat != attribute) continue;
                    yield return modifier;
                }
            }
        }
    }
}