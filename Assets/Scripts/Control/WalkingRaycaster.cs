using System;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    class NavMeshRaycaster : RaycasterBase<Vector3>
    {
        const int POTENTIALLY_WALKABLE_LAYER = 8;

        public NavMeshRaycaster(Action<Vector3> callback) : base(callback)
        {
        }

        protected override bool InteractWithRay(Ray ray, GameObject owner, out Vector3 result)
        {
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
            var raycastHits = Physics.RaycastAll(ray, maxRaycastDepth, potentiallyWalkableLayer);
            foreach (var raycasthit in raycastHits)
            {
                NavMeshHit navMeshHit;
                if (NavMesh.SamplePosition(raycasthit.point, out navMeshHit, 0.2f, NavMesh.AllAreas))
                {
                    result = navMeshHit.position;
                    return true;
                }
            }
            result = default(Vector3);
            return false;
        }
    }
}