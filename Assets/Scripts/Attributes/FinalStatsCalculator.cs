using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class FinalStatsCalculator : MonoBehaviour
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
                return SumModifiersForAttribute(FinalStat.DamageBonus);
            }
        }

        public float criticalHitBonus
        {
            get
            {
                return baseCriticalHit + SumModifiersForAttribute(FinalStat.CriticalHitBonus);
            }
        }

        public float criticalHitChance
        {
            get
            {
                return baseCriticalHitChance + SumModifiersForAttribute(FinalStat.CriticalHitChance);
            }
        }

        public float armour
        {
            get
            {
                return SumModifiersForAttribute(FinalStat.Armour);
            }
        }

        public float armourBonus
        {
            get
            {
                return SumModifiersForAttribute(FinalStat.Armour);
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
            modifierProviders = GetComponents<IStatModifiersProvider>();
        }

        float SumModifiersForAttribute(FinalStat attribute)
        {
            float total = 0;
            foreach (var modifier in GetAttributeModifiersForAttribute(attribute))
            {
                total += modifier.value;
            }
            return total;
        }

        IEnumerable<StatModifier> GetAttributeModifiersForAttribute(FinalStat attribute)
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