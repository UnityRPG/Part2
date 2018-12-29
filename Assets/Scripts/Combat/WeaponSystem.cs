using System.Collections;
using UnityEngine.Assertions;
using UnityEngine;
using RPG.Inventories;
using RPG.Attributes;
using RPG.Core;

namespace RPG.Combat
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig currentWeaponConfig = null;

        GameObject currentTarget;

        GameObject weaponObject;
        Animator animator;
        ActionScheduler actionScheduler;
        Equipment equipment;
        CharacterAttributes attributes;
        Coroutine damageDelay;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        void Start()
        {
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            attributes = GetComponent<CharacterAttributes>();
            equipment = GetComponent<Equipment>();
            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
            UpdateWeapon();
            StartCoroutine(AttackTargetRepeatedly());
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

        public void UpdateWeapon()
        {
            if (equipment)
            {
                currentWeaponConfig = equipment.GetItemInSlot(EquipableItem.EquipLocation.Weapon) as WeaponConfig;
            }

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
            currentTarget = targetToAttack;
        }

        public void StopAttacking()
        {
            currentTarget = null;
        }

        public void Hit()
        {
            Debug.Log("Hit");
        }

        IEnumerator AttackTargetRepeatedly()
        {
            while (true)
            {
                yield return new WaitUntil(() => canAttack);
                while (canAttack)
                {
                    var animationClip = currentWeaponConfig.GetAttackAnimClip();
                    float animationClipTime = animationClip.length / actionScheduler.animSpeedMultiplier;
                    float timeToWait = animationClipTime + currentWeaponConfig.GetTimeBetweenAnimationCycles();
                    if (attributes)
                    {
                        timeToWait = 1 / currentWeaponConfig.GetHitsPerSecond();
                    }

                    AttackTargetOnce();
                    yield return new WaitForSeconds(timeToWait);
                }
            }
        }

        void AttackTargetOnce()
        {
            var action = new SchedulableAction(isInterruptable:false);
            action.OnStart += () =>
            {
                transform.LookAt(currentTarget.transform);
                animator.SetTrigger(ATTACK_TRIGGER);
                float damageDelay = currentWeaponConfig.GetDamageDelay();
                SetAttackAnimation();
                StartCoroutine(DamageAfterDelay(damageDelay, action));
            };
            actionScheduler.QueueAction(action);
        }

        IEnumerator DamageAfterDelay(float delay, SchedulableAction action)
        {
            yield return new WaitForSecondsRealtime(delay);

            if (canAttack)
            {
                currentTarget.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
            }
            action.Finish();
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
    }
}