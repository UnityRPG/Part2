using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class Attributes : MonoBehaviour
    {
        IStatsModifierProvider[] modifierProviders;
        WeaponSystem weaponSystem;
        [SerializeField] float baseCriticalHit = 20;
        [SerializeField] float baseCriticalHitChance = 5;

        public DamageRange totalDamage
        {
            get
            {
                return weaponDamage * (1 + damageBonus / 100f);
            }
        }

        public float damageBonus
        {
            get
            {
                return SumModifiersForAttribute(StatsModifier.Attribute.DamageBonus);
            }
        }

        public DamageRange weaponDamage
        {
            get
            {
                return weaponSystem.GetCurrentWeapon().GetDamageRange();
            }
        }

        public float criticalHitBonus
        {
            get
            {
                return baseCriticalHit + SumModifiersForAttribute(StatsModifier.Attribute.CriticalHitBonus);
            }
        }

        public float criticalHitChance
        {
            get
            {
                return baseCriticalHitChance + SumModifiersForAttribute(StatsModifier.Attribute.CriticalHitChance);
            }
        }

        public float hitSpeed
        {
            get
            {
                return weaponSystem.GetCurrentWeapon().GetHitsPerSecond();
            }
        }

        public float armour
        {
            get
            {
                return SumModifiersForAttribute(StatsModifier.Attribute.Armour);
            }
        }

        public float armourBonus
        {
            get
            {
                return SumModifiersForAttribute(StatsModifier.Attribute.Armour);
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
            weaponSystem = GetComponent<WeaponSystem>();

            modifierProviders = new IStatsModifierProvider[]
            {
                GetComponent<Stats>(),
                GetComponent<PlayerInventory>()
            };
        }

        float SumModifiersForAttribute(StatsModifier.Attribute attribute)
        {
            float total = 0;
            foreach (var modifier in GetStatsModifiersForAttribute(attribute))
            {
                total += modifier.value;
            }
            return total;
        }

        IEnumerable<StatsModifier> GetStatsModifiersForAttribute(StatsModifier.Attribute attribute)
        {
            foreach (var modifierProvider in modifierProviders)
            {
                foreach (var modifier in modifierProvider.modifiers)
                {
                    if (modifier.attribute != attribute) continue;
                    yield return modifier;
                }
            }
        }
    }
}