using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class Stats : MonoBehaviour
    {
        [SerializeField] int strengthPoints;
        [SerializeField] int dexterityPoints;
        [SerializeField] int charismaPoints;
        [SerializeField] int intelligencePoints;
        [SerializeField] int constitutionPoints;

        public DamageRange GetDamage(WeaponConfig weaponUsed)
        {
            return DamageRange.FromPercentage(weaponUsed.GetAdditionalDamage(), 0.1f);
        }

    }
}