using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class AttributeSet : MonoBehaviour, IAttributeModifierProvider
    {
        public enum Attribute
        {
            Damage,
            HitSpeed,
            CriticalHitProbability,
            EnemyHitStrength
        }

        public enum ModifierType
        {
            Multiplicative,
            Additive
        }

        [System.Serializable]
        public struct Modifier
        {
        }

        [Header("Stat Points")]
        [SerializeField] int strengthPoints;
        [SerializeField] int dexterityPoints;
        [SerializeField] int charismaPoints;
        [SerializeField] int intelligencePoints;
        [SerializeField] int constitutionPoints;

        [Header("Stat effects on Attributes")]
        [SerializeField] float damageBonusPerStrengthPoint = 0.5f;
        [SerializeField] float criticalHitBonusPerStrengthPoint = 1.5f;
        [SerializeField] float criticalHitChancePerDexterityPoint = 1.0f;
        [SerializeField] float armourBonusPerConstitutionPoints = 0.5f;

        public IEnumerable<AttributeModifier> modifiers
        {
            get
            {
                return new AttributeModifier[]
                {
                    new AttributeModifier(AttributeModifier.Attribute.DamageBonus, damageBonusPerStrengthPoint * strengthPoints),
                    new AttributeModifier(AttributeModifier.Attribute.CriticalHitBonus, criticalHitBonusPerStrengthPoint * strengthPoints),
                    new AttributeModifier(AttributeModifier.Attribute.CriticalHitChance, criticalHitChancePerDexterityPoint * dexterityPoints),
                    new AttributeModifier(AttributeModifier.Attribute.ArmourBonus, armourBonusPerConstitutionPoints * constitutionPoints)
                };
            }
        }
    }
}