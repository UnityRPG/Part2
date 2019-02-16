using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    abstract class RaycasterBase
    {
        public Texture2D cursorTexture { get; private set; }

        protected const float maxRaycastDepth = 100f; // Hard coded value

        public RaycasterBase(Texture2D cursorTexture)
        {
            this.cursorTexture = cursorTexture;
        }

        public bool Interact(GameObject owner)
        {
            var ray = GetRay();
            if (!ray.HasValue) return false;
            return InteractWithRay(ray.Value, owner);
        }

        protected abstract bool InteractWithRay(Ray ray, GameObject owner);


        private Nullable<Ray> GetRay()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return null;

            var currentScreenRect = new Rect(0, 0, Screen.width, Screen.height);
            if (!currentScreenRect.Contains(Input.mousePosition)) return null;

            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}