using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class CharacterAttributes : MonoBehaviour, IPerformanceModifierProvider
    {
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

        public IEnumerable<PerformanceModifier> modifiers
        {
            get
            {
                return new PerformanceModifier[]
                {
                    new PerformanceModifier(PerformanceModifier.PerformanceStat.DamageBonus, damageBonusPerStrengthPoint * strengthPoints),
                    new PerformanceModifier(PerformanceModifier.PerformanceStat.CriticalHitBonus, criticalHitBonusPerStrengthPoint * strengthPoints),
                    new PerformanceModifier(PerformanceModifier.PerformanceStat.CriticalHitChance, criticalHitChancePerDexterityPoint * dexterityPoints),
                    new PerformanceModifier(PerformanceModifier.PerformanceStat.ArmourBonus, armourBonusPerConstitutionPoints * constitutionPoints)
                };
            }
        }
    }
}