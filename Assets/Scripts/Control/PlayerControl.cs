using UnityEngine;
using System.Collections;
using RPG.Core;
using RPG.Movement;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using RPG.SpecialActions;

namespace RPG.Control
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] CursorMapping[] cursors = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        [System.Serializable]
        public struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
        }

        Mover mover;
        SpecialAbilities abilities;
        const int POTENTIALLY_WALKABLE_LAYER = 8;

        void Start()
        {
            mover = GetComponent<Mover>();
            abilities = GetComponent<SpecialAbilities>();
        }

        void Update()
        {
            ScanForAbilityKeyDown();

            RaycastForTargets();
        }

        private void RaycastForTargets()
        {
            if (RaycastForComponents()) return;
            if (RaycastForWalkable()) return;
            SetCursor(CursorType.None);
        }

        Texture2D GetCursorTexture(CursorType type)
        {
            foreach (var mapping in cursors)
            {
                if (mapping.type == type) return mapping.texture;
            }
            return null;
        }

        void SetCursor(CursorType requestedCursorType)
        {
            Cursor.SetCursor(GetCursorTexture(requestedCursorType), cursorHotspot, CursorMode.Auto);
        }

        void OnWalkPossible(Vector3 destination)
        {
            SetCursor(CursorType.Walk);
            if (Input.GetMouseButton(0))
            {
                mover.StartMovementAction(destination);
            }
        }

        bool RaycastForComponents()
        {
            var ray = GetRay();
            if (!ray.HasValue) return false;

            var raycastHits = Physics.RaycastAll(ray.Value, maxRaycastDepth);
            var raycastables = new List<IRaycastable>();
            foreach (var raycastHit in raycastHits)
            {
                var componentsRaycastables = raycastHit.transform.GetComponents<IRaycastable>();
                raycastables.AddRange(componentsRaycastables);
            }
            raycastables.Sort((x, y) => y.priority.CompareTo(x.priority));
            foreach (var raycastable in raycastables)
            {
                bool handled = raycastable.HandleRaycast(this);
                if (handled) 
                {
                    SetCursor(raycastable.cursor);
                    return true;
                }
            }
            return false;
        }

        protected const float maxRaycastDepth = 100f; // Hard coded value

        private Ray? GetRay()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return null;

            var currentScreenRect = new Rect(0, 0, Screen.width, Screen.height);
            if (!currentScreenRect.Contains(Input.mousePosition)) return null;

            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

    bool RaycastForWalkable()
    {
        var ray = GetRay();
        if (!ray.HasValue) return false;
        LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
        var raycastHits = Physics.RaycastAll(ray.Value, maxRaycastDepth, potentiallyWalkableLayer);
        foreach (var raycasthit in raycastHits)
        {
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(raycasthit.point, out navMeshHit, 0.2f, NavMesh.AllAreas))
            {
                OnWalkPossible(navMeshHit.position);
                return true;
            }
        }
        return false;
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
