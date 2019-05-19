using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Combat;

namespace RPG.SpecialActions
{
    [CreateAssetMenu(menuName = ("RPG/Special Abiltiy/Simple Attack"))]
    public class SimpleAttackConfig : ActionConfig
    {
        [Header("Simple Attack Specific")]
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
            if (target == null) return false;
            return source.GetComponent<WeaponSystem>().hasWeapon;
        }

        public override void Use(GameObject source, GameObject target)
        {
            if (!source.GetComponent<WeaponSystem>().hasWeapon) return;

            source.transform.LookAt(target.transform);
            PlayAbilityAnimation(source, () => { });
            source.GetComponent<SpecialAbilities>().StartCoroutine(EffectsAfterDelay(source, target));
        }

        private IEnumerator EffectsAfterDelay(GameObject source, GameObject target)
        {
            yield return new WaitForSeconds(damageDelay);

            DealDamage(target);
        }

        private void DealDamage(GameObject target)
        {
            float damageToDeal = GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
        }

        public float damageDelay => _damageDelay;

    }
}