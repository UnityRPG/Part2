namespace RPG.Core.Saving
{
    using System.Runtime.Serialization.Formatters.Binary;
    using System.IO;
    using UnityEngine;
    using System.Collections.Generic;
    using LevelState = System.Collections.Generic.Dictionary<string, object>;

    public class SaveLoad : MonoBehaviour
    {

        void Start()
        {
            Load();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                Clear();
            }
        }

        public void Save()
        {
            var levelState = GetLevelState();
            var formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(GetSavePath(), FileMode.Create))
            {
                formatter.Serialize(stream, levelState);
            }
        }

        public bool Load()
        {
            if (!File.Exists(GetSavePath()))
            {
                return false;
            }

            var formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(GetSavePath(), FileMode.Open))
            {
                var levelState = (LevelState)formatter.Deserialize(stream);
                UpdateLevelFromState(levelState);
            }
            return true;
        }

        public void Clear()
        {
            if (File.Exists(GetSavePath()))
            {
                File.Delete(GetSavePath());
            }
        }

        LevelState GetLevelState()
        {
            var saveables = FindObjectsOfType<SaveableEntity>();
            var levelState = new LevelState();
            foreach (var saveable in saveables)
            {
                if (levelState.ContainsKey(saveable.UniqueIdentifier))
                {
                    Debug.LogErrorFormat("Cannot have Saveables with the same name. This id duplicates another: {0}", saveable);

                    continue;
                }

                levelState[saveable.UniqueIdentifier] = saveable.CaptureState();
            }
            return levelState;
        }

        void UpdateLevelFromState(LevelState levelState)
        {
            foreach (KeyValuePair<string, object> entry in levelState)
            {
            }
            var saveables = FindObjectsOfType<SaveableEntity>();
            foreach (var saveable in saveables)
            {
                if (saveable.gameObject.name == "Player")
                {
                    Debug.Log(saveable.UniqueIdentifier);
                    Debug.Log(levelState.ContainsKey(saveable.UniqueIdentifier));
                }
                if (levelState.ContainsKey(saveable.UniqueIdentifier))
                {
                    var saveableState = levelState[saveable.UniqueIdentifier];
                    saveable.RestoreState(saveableState);
                } else
                {
                    Destroy(saveable.gameObject);
                }
            }
        }

        string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, "savegame.sav");
        }
    }
}