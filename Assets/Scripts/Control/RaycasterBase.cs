using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    abstract class RaycasterBase<T> : IRaycastable
    {
        private Action<T> callback { get; set; }

        public RaycasterBase(Action<T> callback) : base()
        {
            this.callback = callback;
        }

        protected const float maxRaycastDepth = 100f; // Hard coded value

        public bool Raycast(GameObject owner)
        {
            var ray = GetRay();
            if (!ray.HasValue) return false;
            T result;
            bool success = InteractWithRay(ray.Value, owner, out result);
            if (success)
            {
                callback(result);
            }
            return success;
        }

        protected abstract bool InteractWithRay(Ray ray, GameObject owner, out T result);

        private Nullable<Ray> GetRay()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return null;

            var currentScreenRect = new Rect(0, 0, Screen.width, Screen.height);
            if (!currentScreenRect.Contains(Input.mousePosition)) return null;

            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}