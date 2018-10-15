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


        public DamageRange GetDamage()
        {
            var baseWeaponDamage = GetComponent<WeaponSystem>().GetCurrentWeapon().GetDamageRange();
            return baseWeaponDamage + strengthPoints;
        }

    }
}