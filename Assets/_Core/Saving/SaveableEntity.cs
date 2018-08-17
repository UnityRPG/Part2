namespace RPG.Core.Saving
{
    using System.Collections.Generic;
    using UnityEngine;

    [ExecuteInEditMode]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField]
        string UUID;

        void Awake()
        {
            if (UUID == "")
            {
                UUID = System.Guid.NewGuid().ToString();
            }
        }

        public string UniqueIdentifier
        {
            get { return UUID; }
        }

        public object CaptureState()
        {
            foreach (var component in GetComponents<MonoBehaviour>())
            { 
            }
            var saveables = GetComponents<ISaveable>();
            var entityState = new Dictionary<string, object>();
            foreach (var saveable in saveables)
            {
                var state = new Dictionary<string, object>();
                saveable.CaptureState(state);
                entityState[saveable.GetType().ToString()] = state;
            }
            return entityState;
        }

        public void RestoreState(object state)
        {
            var entityState = (Dictionary<string, object>) state;
            var saveables = GetComponents<ISaveable>();
            foreach (var saveable in saveables)
            {
                saveable.RestoreState((Dictionary<string, object>)entityState[saveable.GetType().ToString()]);
            }
        }
    }
}