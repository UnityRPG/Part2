﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Combat;

namespace RPG.SpecialActions
{
    [CreateAssetMenu(menuName = ("RPG/Special Abiltiy/Power Attack"))]
    public class PowerAttackConfig : ActionConfig
    {
        [Header("Power Attack Specific")]
        [SerializeField] float extraDamage = 10f;
        [SerializeField] float _damageDelay = 0.2f;

        public float GetExtraDamage()
        {
            return extraDamage;
        }

        public override bool IsInRange(GameObject source, GameObject target)
        {
            if (source.GetComponent<WeaponSystem>() == null) return false;

            float distanceToTarget = (target.transform.position - source.transform.position).magnitude;
            float range = source.GetComponent<WeaponSystem>().GetMaxAttackRange();
            return distanceToTarget <= range;
        }

        public override bool CanUseWhenInRange(GameObject source, GameObject target)
        {
            return source.GetComponent<WeaponSystem>().hasWeapon;
        }

        public override void Use(GameObject source, GameObject target)
        {
            if (!source.GetComponent<WeaponSystem>().hasWeapon) return;

            var action = new SchedulableAction();

            action.OnStart += () =>
            {
                Debug.Log(source);
                Debug.Log(target);
                source.transform.LookAt(target.transform);
                PlayAbilityAnimation(source, () => { });
                source.GetComponent<SpecialAbilities>().StartCoroutine(EffectsAfterDelay(source, action, target));
            };

            source.GetComponent<ActionScheduler>().QueueAction(action);
        }

        private IEnumerator EffectsAfterDelay(GameObject source, SchedulableAction action, GameObject target)
        {
            yield return new WaitForSeconds(damageDelay);

            PlayAbilitySound(source);
            DealDamage(target);
            PlayParticleEffect(source);

            yield return new WaitForSeconds(GetAbilityAnimation().averageDuration - damageDelay);

            action.Finish();
        }

        private void DealDamage(GameObject target)
        {
            float damageToDeal = GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
        }

        public float damageDelay => _damageDelay;
    }
}