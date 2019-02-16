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

        RaycasterBase[] affordanceStrategies;

        Mover mover;
        SpecialAbilities abilities;
        WeaponSystem weaponSystem;

        void Start()
        {
            mover = GetComponent<Mover>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();
            affordanceStrategies = new RaycasterBase[] {
                new ComponentRaycaster<ConversationSource>(talkCursor, AttemptConversation),
                new ComponentRaycaster<EnemyAI>(enemyCursor, AttemptAttack),
                new ComponentRaycaster<Pickup>(pickupCursor, AttemptPickup),
                new NavMeshRaycaster(walkCursor, AttemptWalk)
            };
        }

        void Update()
        {
            ScanForAbilityKeyDown();

            RaycastForAffordanceAndBehaviour();
        }

        private void RaycastForAffordanceAndBehaviour()
        {
            var requestedCursor = defaultCursor;
            foreach (var affordanceStrategy in affordanceStrategies)
            {
                bool canInteract = affordanceStrategy.Interact(gameObject);
                if (canInteract)
                {
                    requestedCursor = affordanceStrategy.cursorTexture;
                    break;
                }
            }
            Cursor.SetCursor(requestedCursor, cursorHotspot, CursorMode.Auto);
        }

        void AttemptWalk(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                CancelAll();
                mover.SetDestination(destination);
            }
        }

        void AttemptAttack(EnemyAI enemy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CancelAll();
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            if (Input.GetMouseButtonDown(1))
            {
                CancelAll();
                abilities.RequestSpecialAbility(0, enemy.gameObject);
            }
        }

        void AttemptConversation(ConversationSource source)
        {
            if (Input.GetMouseButtonDown(0))
            {
                source.VoiceClicked();
            }
        }

        void AttemptPickup(Pickup pickup)
        {
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
                    CancelAll();
                    abilities.RequestSpecialAbility(keyIndex);
                }
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