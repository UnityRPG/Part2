using RPG.Core;
using System.Collections;
using UnityEngine;

namespace RPG.SpecialActions
{
    public abstract class ActionBehaviour : MonoBehaviour
    {
        protected ActionConfig config;

        public abstract void Use(GameObject target = null);

        public virtual bool IsInRange(GameObject target) => true;

        public virtual bool CanUseWhenInRange(GameObject target) => true;

        public void SetConfig(ActionConfig configToSet)
        {
            config = configToSet;
        }

    }
}