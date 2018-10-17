using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class Attributes : MonoBehaviour
    {
        IStatsModifierProvider[] modifierProviders;
        WeaponSystem weaponSystem;

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
                return 0;
            }
        }

        public float criticalHitChance
        {
            get
            {
                return 0;
            }
        }

        public float hitSpeed
        {
            get
            {
                return weaponSystem.GetCurrentWeapon().GetHitsPerSecond();
            }
        }

        public float hitSpeedBonus
        {
            get
            {
                return SumModifiersForAttribute(StatsModifier.Attribute.HitSpeedBonus);
            }
        }

        public float totalHitSpeed
        {
            get
            {
                return hitSpeed * (1 + hitSpeedBonus / 100f);
            }
        }

        public float armour
        {
            get
            {
                return 0;
            }
        }

        public float armourBonus
        {
            get
            {
                return 0;
            }
        }

        public float totalDefence
        {
            get
            {
                return 0;
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
                var modifiers = modifierProvider.GetModifiersForAttribute(attribute);
                foreach (var modifier in modifiers)
                {
                    if (modifier.attribute != attribute) continue;
                    yield return modifier;
                }
            }
        }
    }
}