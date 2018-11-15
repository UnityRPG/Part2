namespace RPG.Core.Saving
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.SceneManagement;

    [ExecuteInEditMode]
    public class SaveableEntity : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField]
        string UUID = null;

        public string UniqueIdentifier
        {
            get { return UUID; }
        }

        public Dictionary<string, object> CaptureState()
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

        public void RestoreState(Dictionary<string, object> entityState)
        {

            var saveables = GetComponents<ISaveable>();
            foreach (var saveable in saveables)
            {
                saveable.RestoreState(GetSaveableState(entityState, saveable));
            }
        }

        private static Dictionary<string, object> GetSaveableState(Dictionary<string, object> entityState, ISaveable saveable)
        {
            var saveableName = saveable.GetType().ToString();
            if (entityState.ContainsKey(saveableName))
            {
                return (Dictionary<string, object>)entityState[saveableName];
            }

            return new Dictionary<string, object>();
        }

        public void OnBeforeSerialize()
        {
            // Part of scene and not a prefab
            bool isPartOfScene = string.IsNullOrEmpty(gameObject.scene.path);
            if (isPartOfScene)
            {
                UUID = null;
            }
            else if (string.IsNullOrEmpty(UUID))
            {
                UUID = System.Guid.NewGuid().ToString();
            }
        }

        public void OnAfterDeserialize()
        {
        }
    }
}