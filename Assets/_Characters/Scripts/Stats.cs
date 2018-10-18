using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class Stats : MonoBehaviour, IStatsModifierProvider
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

        public IEnumerable<StatsModifier> modifiers
        {
            get
            {
                return new StatsModifier[]
                {
                    new StatsModifier(StatsModifier.Attribute.DamageBonus, damageBonusPerStrengthPoint * strengthPoints),
                    new StatsModifier(StatsModifier.Attribute.CriticalHitBonus, criticalHitBonusPerStrengthPoint * strengthPoints),
                    new StatsModifier(StatsModifier.Attribute.CriticalHitChance, criticalHitChancePerDexterityPoint * dexterityPoints),
                    new StatsModifier(StatsModifier.Attribute.ArmourBonus, armourBonusPerConstitutionPoints * constitutionPoints)
                };
            }
        }
    }
}