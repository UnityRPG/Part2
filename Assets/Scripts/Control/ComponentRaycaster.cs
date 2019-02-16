using System;
using UnityEngine;

namespace RPG.Control
{
    class ComponentRaycaster<C> : RaycasterCallbackBase<C>
    {
        public ComponentRaycaster(Texture2D cursorTexture, Action<C> callback) : base(cursorTexture, callback)
        {
        }

        protected override bool InteractWithRay(Ray ray, GameObject owner)
        {
                GameObject gameObjectHit = GetFirstHitGameObject(ray);
                var hit = gameObjectHit.GetComponent<C>();
                if (hit != null)
                {
                    callback(hit);
                    return true;
                }
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
