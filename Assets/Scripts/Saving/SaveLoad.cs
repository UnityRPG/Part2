namespace RPG.Saving
{
    using System.Runtime.Serialization.Formatters.Binary;
    using System.IO;
    using UnityEngine;
    using System.Collections.Generic;
    using System;

    public class SaveLoad : MonoBehaviour
    {

        [SerializeField]
        [Tooltip("In seconds")]
        float AutoSaveInterval = 60;

        float TimeSinceLastSave = 0;

        void Start()
        {
            Load(GetLastSaveFile());
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load(GetLastSaveFile());
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                Clear();
            }

            HandleAutoSave();
        }

        public void Save(bool isAuto = false)
        {
            string saveFile = GetSaveFile(isAuto);
            GetComponent<SaveSystem>().Save(saveFile);
        }

        public bool Load(string saveFile)
        {
            return GetComponent<SaveSystem>().Load(saveFile);
        }

        public string[] GetSaveFileList()
        {
            return GetComponent<SaveSystem>().GetSaveFileList();
        }

        public string GetLastSaveFile()
        {
            var saveFiles = GetSaveFileList();
            string lastSaveFile = null;
            DateTime lastSaveFileWriteTime = DateTime.MinValue;
            foreach (var saveFile in saveFiles)
            {
                string path = GetComponent<SaveSystem>().GetPathFromSaveFile(saveFile);
                var writeTime = File.GetLastWriteTime(path);
                if (writeTime > lastSaveFileWriteTime)
                {
                    lastSaveFileWriteTime = writeTime;
                    lastSaveFile = saveFile;
                }
            }
            return lastSaveFile;
        }

        public void Clear()
        {
            GetComponent<SaveSystem>().Delete(GetSaveFile());
        }

        private void HandleAutoSave()
        {
            TimeSinceLastSave += Time.deltaTime;
            if (TimeSinceLastSave > AutoSaveInterval)
            {
                TimeSinceLastSave = 0;
                Save(isAuto: true);
            }
        }

        string GetSaveFile(bool isAuto = true)
        {
            var prefix = "Manual Save";
            if (isAuto)
            {
                prefix = "Auto Save";
            }
            var dateString = DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss");
            return String.Format("{0} {1}", prefix, dateString);
        }
    }
}