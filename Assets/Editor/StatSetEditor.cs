using UnityEngine;
using UnityEditor;

namespace RPG.Core.Stats
{
    [CustomEditor(typeof(StatSet))]
    public class StatSetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (GUILayout.Button("Import CSV"))
            {
                string path = EditorUtility.OpenFilePanel("Import CSV", "", "csv");
                Import(path);
            }


            serializedObject.ApplyModifiedProperties();
            DrawDefaultInspector();
        }

        private void Import(string path)
        {
            var importer = new CSVImporter(path);
            importer.Load();

            var property = serializedObject.FindProperty("playerLevels");

            property.arraySize = GetNumberOfLevels(importer);

            string statString = "Total Hit Damage";
            int row = GetRowForStat(importer, "Player", statString);
            if (row == -1)
            {
                return;
            }

            for (int i = 0; i < property.arraySize; i++)
            {
                string stat = importer.GetCell((i + 1).ToString(), row);

                float floatStat;
                if (float.TryParse(stat, out floatStat))
                {
                    Debug.LogFormat(this, "Stat: {0}, {1}", i, stat);
                    var levelProperty = property.GetArrayElementAtIndex(i);
                    var healthProperty = levelProperty.FindPropertyRelative("health");
                    healthProperty.floatValue = floatStat;
                }
            }
            Debug.LogFormat("import health[0]: {0}", serializedObject.FindProperty("playerLevels").GetArrayElementAtIndex(0).FindPropertyRelative("health").floatValue);

        }

        private int GetNumberOfLevels(CSVImporter importer)
        {
            for (int level = 0; true; level++)
            {
                int column = importer.GetIndexForHeader((level + 1).ToString());
                if (column == -1)
                {
                    return level;
                }
            }
        }

        private int GetRowForStat(CSVImporter importer, string playerClass, string statString)
        {
            for (int i = 0; i < importer.GetLength(); i++)
            {
                string className = importer.GetCell("Class", i);
                if (className != playerClass) continue;

                string statName = importer.GetCell("Player Details", i);
                if (statName != statString) continue;

                return i;
            }

            return -1;
        }

    }
}