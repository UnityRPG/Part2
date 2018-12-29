using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Inventories;

namespace RPG.Attributes
{
    public class CharacterAttributes : MonoBehaviour
    {
        IAttributeModifierProvider[] modifierProviders;
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
                return SumModifiersForAttribute(AttributeModifier.Attribute.DamageBonus);
            }
        }

        public float criticalHitBonus
        {
            get
            {
                return baseCriticalHit + SumModifiersForAttribute(AttributeModifier.Attribute.CriticalHitBonus);
            }
        }

        public float criticalHitChance
        {
            get
            {
                return baseCriticalHitChance + SumModifiersForAttribute(AttributeModifier.Attribute.CriticalHitChance);
            }
        }

        public float armour
        {
            get
            {
                return SumModifiersForAttribute(AttributeModifier.Attribute.Armour);
            }
        }

        public float armourBonus
        {
            get
            {
                return SumModifiersForAttribute(AttributeModifier.Attribute.Armour);
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
            modifierProviders = new IAttributeModifierProvider[]
            {
                GetComponent<AttributeSet>(),
                GetComponent<Equipment>()
            };
        }

        float SumModifiersForAttribute(AttributeModifier.Attribute attribute)
        {
            float total = 0;
            foreach (var modifier in GetAttributeModifiersForAttribute(attribute))
            {
                total += modifier.value;
            }
            return total;
        }

        IEnumerable<AttributeModifier> GetAttributeModifiersForAttribute(AttributeModifier.Attribute attribute)
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