﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            if (!GetComponent<WeaponSystem>().hasWeapon) return;

            PlayAbilityAnimation(() =>
            {
                PlayAbilitySound();
                DealDamage(target);
                PlayParticleEffect();
            });
        }

        public override bool CanUseWhenInRange(GameObject target)
        {
            return GetComponent<WeaponSystem>().hasWeapon;
        }

        public override bool IsInRange(GameObject target)
        {
            if (GetComponent<WeaponSystem>() == null) return false;

            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            float range = GetComponent<WeaponSystem>().GetMaxAttackRange();
            return distanceToTarget <= range;
        }

        private void DealDamage(GameObject target)
        {
            float damageToDeal = (config as PowerAttackConfig).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
        }
    }
}