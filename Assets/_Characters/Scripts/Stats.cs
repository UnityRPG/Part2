using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class Stats : MonoBehaviour
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
            public Attribute attribute;
            public ModifierType modifierType;
            public float value;
        }

        [SerializeField] int strengthPoints;
        [SerializeField] int dexterityPoints;
        [SerializeField] int charismaPoints;
        [SerializeField] int intelligencePoints;
        [SerializeField] int constitutionPoints;


        public DamageRange GetDamage()
        {
            var baseWeaponDamage = GetComponent<WeaponSystem>().GetCurrentWeapon().GetDamageRange();
            var inventory = GetComponent<PlayerInventory>();
            return (baseWeaponDamage + inventory.GetAdditiveModifiers(Attribute.Damage)) * (1 + strengthPoints / 100f) * inventory.GetMultiplicativeModifiers(Attribute.Damage);
        }

    }
}