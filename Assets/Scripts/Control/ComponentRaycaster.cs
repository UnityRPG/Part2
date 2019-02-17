using System;
using UnityEngine;

namespace RPG.Control
{
    class ComponentRaycaster<C> : RaycasterBase<C> where C : Component
    {
        public ComponentRaycaster(Action<C> callback) : base(callback)
        {
        }

        protected override bool InteractWithRay(Ray ray, GameObject owner, out C result)
        {
            GameObject gameObjectHit = GetFirstHitGameObject(ray);
            var hit = gameObjectHit.GetComponent<C>();
            if (hit != null)
            {
                result = hit;
                return true;
            }
            result = null;
            return false;
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
