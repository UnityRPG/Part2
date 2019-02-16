using System;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    class NavMeshRaycaster : RaycasterCallbackBase<Vector3>
    {
        const int POTENTIALLY_WALKABLE_LAYER = 8;

        public NavMeshRaycaster(Texture2D cursorTexture, Action<Vector3> callback) : base(cursorTexture, callback)
        {
        }

        protected override bool InteractWithRay(Ray ray, GameObject owner)
        {
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
            var raycastHits = Physics.RaycastAll(ray, maxRaycastDepth, potentiallyWalkableLayer);
            foreach (var raycasthit in raycastHits)
            {
                NavMeshHit navMeshHit;
                if (NavMesh.SamplePosition(raycasthit.point, out navMeshHit, 0.2f, NavMesh.AllAreas))
                {
                    callback(navMeshHit.position);
                    return true;
                }
            }
            return false;
        }
    }
}