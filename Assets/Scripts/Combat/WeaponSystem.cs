﻿using System.Collections;
using UnityEngine.Assertions;
using UnityEngine;
using RPG.Stats;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat
{
    public class WeaponSystem : MonoBehaviour, ISchedulableAction
    {
        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig currentWeaponConfig = null;

        GameObject desiredTarget;
        GameObject currentTarget;

        GameObject weaponObject;
        Animator animator;
        Mover mover;
        ActionScheduler actionScheduler;
        StatsCalculator attributes;
        Coroutine damageDelay;
        float timeTillNextAttack;

        bool stopRequested = false;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        void Start()
        {
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            attributes = GetComponent<StatsCalculator>();

            UpdateWeapon(currentWeaponConfig);
        }

        private void Update() {
            if (CanAttackWhenInRange(currentTarget))
            {
                if (TargetIsOutOfRange(currentTarget))
                {
                    mover.StartMoving(currentTarget.transform.position);
                }
                else
                {
                    mover.StopMoving();
                    AttackBehaviour();
                }
            }
            else 
            {
                StopExecuted();
            }
            timeTillNextAttack -= Time.deltaTime;
        }

        public bool CanAttackWhenInRange(GameObject target)
        {
            return target != null && hasWeapon && !characterIsDead && !TargetIsDead(target);
        }

        public bool canAttack => CanAttackWhenInRange(currentTarget) && !TargetIsOutOfRange(currentTarget);

        public bool hasWeapon => currentWeaponConfig != null;

        public bool characterIsDead {
            get
            {
                float characterHealth = GetComponent<HealthSystem>().healthAsPercentage;
                return (characterHealth <= Mathf.Epsilon);
            }
        }

        public bool TargetIsOutOfRange(GameObject target)
        {
            if (target == null) return false;

            if (currentWeaponConfig != null)
            {
                var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                return distanceToTarget > currentWeaponConfig.GetMaxAttackRange();
            }
            return true;
        }

        public bool TargetIsDead(GameObject target)
        {
            if (target == null) return false;

            var targethealth = target.GetComponent<HealthSystem>().healthAsPercentage;
            return targethealth <= Mathf.Epsilon;
        }

        public void UpdateWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;

            Destroy(weaponObject); // empty hands

            if (currentWeaponConfig != null)
            {
                var weaponPrefab = currentWeaponConfig.GetWeaponPrefab();
                GameObject dominantHand = RequestDominantHand();
                weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
                weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
                weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
            }
        }

        public void AttackTarget(GameObject targetToAttack)
        {
            desiredTarget = targetToAttack;

            actionScheduler.QueueAction(this);
        }


        void ISchedulableAction.Start()
        {
            currentTarget = desiredTarget;
        }

        void ISchedulableAction.RequestCancel()
        {
            StopAttacking();
        }

        public void StopAttacking()
        {
            stopRequested = true;
        }

        public void Hit()
        {
            Debug.Log("Hit");
        }

        void AttackBehaviour()
        {
            if (timeTillNextAttack > 0) return;

            var animationClip = currentWeaponConfig.GetAttackAnimClip();
            float animationClipTime = animationClip.length / actionScheduler.animSpeedMultiplier;
            timeTillNextAttack = animationClipTime + currentWeaponConfig.GetTimeBetweenAnimationCycles();
            if (attributes)
            {
                timeTillNextAttack = 1 / currentWeaponConfig.GetHitsPerSecond();
            }
            float percentage_of_time = timeTillNextAttack * 0.1f;
            timeTillNextAttack += Random.Range(-percentage_of_time, percentage_of_time);

            AttackTargetOnce();
        }

        void AttackTargetOnce()
        {
            transform.LookAt(currentTarget.transform);
            animator.SetTrigger(ATTACK_TRIGGER);
            float damageDelay = currentWeaponConfig.GetDamageDelay();
            SetAttackAnimation();
            StartCoroutine(DamageAfterDelay(damageDelay));
        }

        IEnumerator DamageAfterDelay(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);

            if (canAttack)
            {
                currentTarget.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
            }
            if (stopRequested)
            {
                StopExecuted();
            }
        }

        private void StopExecuted()
        {
            actionScheduler.FinishAction(this);
            currentTarget = null;
            stopRequested = false;
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }

        public float GetMaxAttackRange() 
        {
            if (currentWeaponConfig == null) return 0;
            return currentWeaponConfig.GetMaxAttackRange();
        }

        void SetAttackAnimation()
        {
            if (currentWeaponConfig == null) return;

            var animatorOverrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (!animatorOverrideController)
            {
                Debug.Break();
                Debug.LogAssertion("Please provide " + gameObject + " with an animator override controller.");
            }
            else
            {
                animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip();
            }
        }

        GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on " + gameObject.name + ", please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on " + gameObject.name + ", please remove one");
            return dominantHands[0].gameObject;
        }

        float CalculateDamage()
        {
            if (attributes == null)
            {
                return baseDamage + currentWeaponConfig.GetAdditionalDamage();
            }

            float weaponDamage = currentWeaponConfig.GetDamageRange().RandomlyChooseDamage();
            float attributeModified = weaponDamage * attributes.damageMultiplier;
            bool shouldBeCritical = Random.value < attributes.criticalHitChance / 100;
            float bonus = shouldBeCritical ? attributes.criticalHitBonus : 0;
            float critcalApplied = attributeModified * (1 + bonus / 100);
            return critcalApplied;
        }

        void OnDrawGizmos()
        {
            if (currentWeaponConfig)
            {
                // Draw attack sphere 
                Gizmos.color = new Color(255f, 0, 0, .5f);
                Gizmos.DrawWireSphere(transform.position, currentWeaponConfig.GetMaxAttackRange());
            }
        }
    }
}