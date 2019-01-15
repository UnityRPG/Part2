﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.SpecialActions
{
    [CreateAssetMenu(menuName = ("RPG/Special Abiltiy/Area Effect"))]
    public class AreaEffectConfig : ActionConfig
    {
        [Header("Area Effect Specific")]
        [SerializeField] float radius = 5f;
        [SerializeField] float damageToEachTarget = 15f;

        public float GetDamageToEachTarget()
        {
            return damageToEachTarget;
        }

        public float GetRadius()
        {
            return radius;
        }

        public override void Use(GameObject source, GameObject target)
        {
            PlayAbilityAnimation(source, () =>
            {
                PlayAbilitySound(source);
                DealRadialDamage(source);
                PlayParticleEffect(source);
            });
        }

        private void DealRadialDamage(GameObject source)
        {
            // Static sphere cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(
                source.transform.position,
                GetRadius(),
                Vector3.up,
                GetRadius()
            );

            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<HealthSystem>();
                bool hitPlayer = hit.collider.gameObject.tag == "Player";
                if (damageable != null && !hitPlayer)
                {
                    float damageToDeal = GetDamageToEachTarget();
                    damageable.TakeDamage(damageToDeal);
                }
            }
        }
    }
}