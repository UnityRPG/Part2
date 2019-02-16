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

        const int POTENTIALLY_WALKABLE_LAYER = 8;
        const float maxRaycastDepth = 100f; // Hard coded value

        Mover mover;
        SpecialAbilities abilities;
        WeaponSystem weaponSystem;

        Texture2D requestedCursor;

        void Start()
        {
            mover = GetComponent<Mover>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();
        }

        void Update()
        {
            ScanForAbilityKeyDown();

            requestedCursor = defaultCursor;

            PerformRaycasts();
            Cursor.SetCursor(requestedCursor, cursorHotspot, CursorMode.Auto);
        }

        private void PerformRaycasts()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            var currentScreenRect = new Rect(0, 0, Screen.width, Screen.height);
            if (!currentScreenRect.Contains(Input.mousePosition)) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Specify layer priorities below, order matters
            ConversationSource conversation = RaycastForComponent<ConversationSource>(ray, talkCursor);
            if (conversation != null) 
            {
                if (Input.GetMouseButtonDown(0))
                {
                    conversation.VoiceClicked();
                }
            }
            
            EnemyAI enemy = RaycastForComponent<EnemyAI>(ray, enemyCursor);
            if (enemy != null) 
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Attack(enemy.gameObject);
                }
                if (Input.GetMouseButtonDown(1))
                {
                    QuickSpecialAbility(enemy.gameObject);
                }
                return;
            }

            Pickup pickup = RaycastForComponent<Pickup>(ray, pickupCursor);
            if (pickup != null) {
                if (Input.GetMouseButtonDown(0))
                {
                    pickup.PickupItem();
                }
                return;
            }

            var location = NavigateToWalkable(ray);
            if (location.HasValue) {
                if (Input.GetMouseButton(0))
                {
                    Walk(location.Value);
                }
                return;    
            }
        }

        void Walk(Vector3 destination)
        {
            CancelAll();
            mover.SetDestination(destination);
        }

        void Attack(GameObject target)
        {
            CancelAll();
            weaponSystem.AttackTarget(target);
        }

        void QuickSpecialAbility(GameObject target)
        {
            CancelAll();
            abilities.RequestSpecialAbility(0, target);
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

        private C RaycastForComponent<C>(Ray ray, Texture2D cursor) where C : Component
        {
            GameObject gameObjectHit = GetFirstHitGameObject(ray);
            var hit = gameObjectHit.GetComponent<C>();
            if (hit != null)
            {
                requestedCursor = cursor;
                return hit;
            }
            return null;
        }

        private Nullable<Vector3> NavigateToWalkable(Ray ray)
        {
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
            var raycastHits = Physics.RaycastAll(ray, maxRaycastDepth, potentiallyWalkableLayer);
            foreach (var raycasthit in raycastHits)
            {
                NavMeshHit navMeshHit;
                if (NavMesh.SamplePosition(raycasthit.point, out navMeshHit, 0.2f, NavMesh.AllAreas))
                {
                    requestedCursor = walkCursor;
                    return navMeshHit.position;
                }
            }
            return null;
        }

        private GameObject GetFirstHitGameObject(Ray ray)
        {
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            var gameObjectHit = hitInfo.collider.gameObject;
            return gameObjectHit;
        }
    }
}