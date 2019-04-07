using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using LevelState = System.Collections.Generic.Dictionary<string, object>;

namespace RPG.Saving
{
    public class SaveSystem : MonoBehaviour
    {
        SaveableEntity[] saveables;


        private void Awake() {
            saveables = FindObjectsOfType<SaveableEntity>();
        }

        public void Save(string saveFile)
        {
            var levelState = GetLevelState();
            var formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(GetPathFromSaveFile(saveFile), FileMode.Create))
            {
                formatter.Serialize(stream, levelState);
            }
        }

        public bool Load(string saveFile)
        {
            var savePath = GetPathFromSaveFile(saveFile);
            if (!File.Exists(savePath))
            {
                return false;
            }

            var formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(savePath, FileMode.Open))
            {
                var levelState = (LevelState)formatter.Deserialize(stream);
                UpdateLevelFromState(levelState);
            }
            return true;
        }

        public string[] GetSaveFileList()
        {
            var filePaths = Directory.GetFiles(Application.persistentDataPath);
            var fileNames = new string[filePaths.Length];
            for (int i = 0; i < filePaths.Length; ++i)
            {
                fileNames[i] = Path.GetFileNameWithoutExtension(filePaths[i]);
            }
            return fileNames;
        }

        public string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, String.Format("{0}.sav", saveFile));
        }

        public bool Delete(string saveFile)
        {
            if (File.Exists(GetPathFromSaveFile(saveFile)))
            {
                File.Delete(GetPathFromSaveFile(saveFile));
                return true;
            }
            return false;
        }

        LevelState GetLevelState()
        {
            var levelState = new LevelState();
            var debugState = new LevelState();
            foreach (SaveableEntity saveable in saveables)
            {
                if (levelState.ContainsKey(saveable.UniqueIdentifier))
                {
                    Debug.LogErrorFormat("Cannot have Saveables with the same name. This id duplicates another: {0}, {1}", saveable, debugState[saveable.UniqueIdentifier]);

                    continue;
                }

                levelState[saveable.UniqueIdentifier] = saveable.CaptureState();
                debugState[saveable.UniqueIdentifier] = saveable;
            }
            return levelState;
        }

        private void UpdateLevelFromState(LevelState levelState)
        {
            foreach (SaveableEntity saveable in saveables)
            {
                var saveableState = GetSaveableState(levelState, saveable);
                saveable.RestoreState(saveableState);
            }
        }

        private static Dictionary<string, object> GetSaveableState(LevelState levelState, SaveableEntity saveable)
        {
            if (levelState.ContainsKey(saveable.UniqueIdentifier))
            {
                return (Dictionary<string, object>)levelState[saveable.UniqueIdentifier];
            }

            return new Dictionary<string, object>();
        }
    }
}