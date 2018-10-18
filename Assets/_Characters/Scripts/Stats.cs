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

        [SerializeField] int strengthPoints;
        [SerializeField] int dexterityPoints;
        [SerializeField] int charismaPoints;
        [SerializeField] int intelligencePoints;
        [SerializeField] int constitutionPoints;

        public IEnumerable<StatsModifier> modifiers
        {
            get
            {
                return new StatsModifier[0];
            }
        }
    }
}