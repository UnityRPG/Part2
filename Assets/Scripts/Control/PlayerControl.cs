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
        }

        void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    CancelAll();
                    abilities.RequestSpecialAbility(keyIndex);
                }
            }
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                CancelAll();
                mover.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(EnemyAI enemy)
        {
            if (Input.GetMouseButton(0))
            {
                CancelAll();
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                CancelAll();
                abilities.RequestSpecialAbility(0, enemy.gameObject);
            }
        }

        void CancelAll()
        {
            weaponSystem.StopAttacking();
            abilities.CancelRequest();
            mover.ClearDestination();
        }
    }
}