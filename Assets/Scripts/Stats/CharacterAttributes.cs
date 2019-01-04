using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class CharacterAttributes : MonoBehaviour, IStatModifiersProvider
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

        public IEnumerable<StatModifier> modifiers
        {
            get
            {
                return new StatModifier[]
                {
                    new StatModifier("damage", damageBonusPerStrengthPoint * strengthPoints, StatModifier.AggregationType.PercentageBonus),
                    new StatModifier("criticalHitBonus", criticalHitBonusPerStrengthPoint * strengthPoints),
                    new StatModifier("criticalHitChance", criticalHitChancePerDexterityPoint * dexterityPoints),
                    new StatModifier("armour", armourBonusPerConstitutionPoints * constitutionPoints, StatModifier.AggregationType.PercentageBonus)
                };
            }
        }
    }
}