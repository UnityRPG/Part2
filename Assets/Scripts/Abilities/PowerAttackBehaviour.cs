﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Combat;

namespace RPG.Abilities
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            if (!GetComponent<WeaponSystem>().hasWeapon) return;

            var action = new SchedulableAction(false);

            action.OnStart += () =>
            {
                transform.LookAt(target.transform);
                PlayAbilityAnimation(() => {});
                StartCoroutine(EffectsAfterDelay(action, target));
            };

            GetComponent<ActionScheduler>().QueueAction(action);
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

        private IEnumerator EffectsAfterDelay(SchedulableAction action, GameObject target)
        {
            float delay =  (config as PowerAttackConfig).damageDelay;
            yield return new WaitForSeconds(delay);

            PlayAbilitySound();
            DealDamage(target);
            PlayParticleEffect();

            yield return new WaitForSeconds(config.GetAbilityAnimation().averageDuration - delay);

            action.Finish();
        }

        private void DealDamage(GameObject target)
        {
            float damageToDeal = (config as PowerAttackConfig).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
        }
    }
}