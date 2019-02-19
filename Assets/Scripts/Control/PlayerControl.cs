using UnityEngine;
using System.Collections;
using RPG.Core;
using RPG.Combat;
using RPG.Movement;
using RPG.SpecialActions;
using RPG.Dialogue;
using RPG.Inventories;
using UnityEngine.AI;
using System;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] Texture2D defaultCursor = null;
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Texture2D pickupCursor = null;
        [SerializeField] Texture2D talkCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        IRaycastable[] raycasters;

        Mover mover;
        SpecialAbilities abilities;
        WeaponSystem weaponSystem;

        void Start()
        {
            mover = GetComponent<Mover>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();
            raycasters = new IRaycastable[] {
                new ComponentRaycaster<ConversationSource>(OnConversationPossible),
                new ComponentRaycaster<EnemyAI>(OnAttackPossible),
                new ComponentRaycaster<Pickup>(OnPickupPossible),
                new NavMeshRaycaster(OnWalkPossible)
            };
        }

        void Update()
        {
            ScanForAbilityKeyDown();

            TriggerRaycastCallbacks();
        }

        private void TriggerRaycastCallbacks()
        {
            foreach (var raycaster in raycasters)
            {
                if (raycaster.Raycast(gameObject)) return;
            }
            SetCursor(defaultCursor);
        }

        private void SetCursor(Texture2D requestedCursor)
        {
            Cursor.SetCursor(requestedCursor, cursorHotspot, CursorMode.Auto);
        }

        void OnWalkPossible(Vector3 destination)
        {
            SetCursor(walkCursor);
            if (Input.GetMouseButton(0))
            {
                mover.StartMovementAction(destination);
            }
        }

        void OnAttackPossible(EnemyAI enemy)
        {
            SetCursor(enemyCursor);

            if (Input.GetMouseButtonDown(0))
            {
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            if (Input.GetMouseButtonDown(1))
            {
                abilities.RequestSpecialAbility(0, enemy.gameObject);
            }
        }

        void OnConversationPossible(ConversationSource source)
        {
            SetCursor(talkCursor);
            
            if (Input.GetMouseButtonDown(0))
            {
                source.VoiceClicked();
            }
        }

        void OnPickupPossible(Pickup pickup)
        {
            SetCursor(pickupCursor);
            
            if (Input.GetMouseButtonDown(0))
            {
                pickup.PickupItem();
            }
        }

        void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    abilities.RequestSpecialAbility(keyIndex);
                }
            }
        }
    }
}