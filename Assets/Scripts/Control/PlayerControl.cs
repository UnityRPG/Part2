using UnityEngine;
using System.Collections;
using RPG.Core;
using RPG.Combat;
using RPG.Movement;
using RPG.SpecialActions;

namespace RPG.Control
{
    public class PlayerControl : MonoBehaviour
    {
        Mover mover;
        SpecialAbilities abilities;
        WeaponSystem weaponSystem;

        int desiredSpecialAbility = -1;
        bool wantsToMove = false;
        Vector3 desiredLocation;
        GameObject target;

        void Start()
        {
            mover = GetComponent<Mover>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();
            
            RegisterForMouseEvents();
        }

        private void RegisterForMouseEvents()
        {
            var cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }

        void Update()
        {
            ScanForAbilityKeyDown();

            weaponSystem.StopAttacking();
            mover.ClearDestination();

            if (desiredSpecialAbility != -1)
            {
                PerformSpecialAbilityBehaviour();
            }
            else if (target)
            {
                PerformAttackBehaviour();
            }
            else if (wantsToMove)
            {
                mover.SetDestination(desiredLocation);
            }
        }

        private void PerformSpecialAbilityBehaviour()
        {
            if (!abilities.CanUseWhenInRange(desiredSpecialAbility, target))
            {
                desiredSpecialAbility = -1;
            }

            if (target != null)
            {
                PerformTargettedSpecialAbilityBehaviour();
            }
            else
            {
                AttemptAbility();
            }
        }

        private void PerformAttackBehaviour()
        {
            if (!weaponSystem.CanAttackWhenInRange(target)) return;

            if (weaponSystem.TargetIsOutOfRange(target))
            {
                mover.SetDestination(target.transform.position);
            }
            else
            {
                weaponSystem.AttackTarget(target);
            }
        }

        private void PerformTargettedSpecialAbilityBehaviour()
        {
            if (!abilities.IsInRange(desiredSpecialAbility, target))
            {
                mover.SetDestination(target.transform.position);
            }
            else
            {
                AttemptAbility(target);
            }
        }

        private void AttemptAbility(GameObject target = null)
        {
            abilities.AttemptSpecialAbility(desiredSpecialAbility, target);
            desiredSpecialAbility = -1;
            return;
        }

        void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    desiredSpecialAbility = keyIndex;
                }
            }
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                wantsToMove = true;
                target = null;
                desiredLocation = destination;
            }
        }

        void OnMouseOverEnemy(EnemyAI enemy)
        {
            if (Input.GetMouseButton(0))
            {
                target = enemy.gameObject;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                target = enemy.gameObject;
                desiredSpecialAbility = 0;
            }
        }
    }
}