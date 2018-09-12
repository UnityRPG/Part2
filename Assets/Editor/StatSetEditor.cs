using UnityEngine;
using UnityEditor;

namespace RPG.Core.Stats
{
    [CustomEditor(typeof(StatSet))]
    public class StatSetEditor : UnityEditor.Editor
    {

        CSVImporter currentImporter;

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
            currentImporter = new CSVImporter(path);
            currentImporter.Load();

            ImportPlayer();
            ImportEnemies();

        }

        private void ImportPlayer()
        {
            var property = serializedObject.FindProperty("playerLevels");

            ImportProperty(property, "Base Health", "Player", "health");
            ImportProperty(property, "Base Damage (per hit)", "Player", "damagePerHit");
            ImportProperty(property, "Typical Ability Damage", "Player", "specialDamage");

            ImportProperty(property, "XP to level up", "Player", "XPToLevelUp");
            ImportProperty(property, "Experience per enemy kill", "Player", "XPPerEnemyKill");
        }

        private void ImportEnemies()
        {
            var property = serializedObject.FindProperty("enemyClasses");

            for (int i = 0; i < property.arraySize; i++)
            {
                var enemyClassProperty = property.GetArrayElementAtIndex(i);
                var enemyClassName = enemyClassProperty.FindPropertyRelative("className").stringValue;
                var levelsProperty = enemyClassProperty.FindPropertyRelative("enemyLevels");

                ImportProperty(levelsProperty, "Health", enemyClassName, "health");
                ImportProperty(levelsProperty, "Damage per hit", enemyClassName, "damagePerHit");
                ImportProperty(levelsProperty, "Hit speed / DPS", enemyClassName, "hitsPerSecond");
                ImportProperty(levelsProperty, "Big Hit / special", enemyClassName, "specialDamage");
            }

        }

        private void ImportProperty(SerializedProperty property, string statString, string characterClass, string propertyName)
        {
            property.arraySize = GetNumberOfLevels();

            int row = GetRowForStat(characterClass, statString);
            if (row == -1)
            {
                return;
            }

            for (int i = 0; i < property.arraySize; i++)
            {
                string stat = currentImporter.GetCell((i + 1).ToString(), row);

                var levelProperty = property.GetArrayElementAtIndex(i);
                var healthProperty = levelProperty.FindPropertyRelative(propertyName);

                float floatStat;
                if (float.TryParse(stat, out floatStat))
                {
                    healthProperty.floatValue = floatStat;
                }
            }
        }

        private int GetNumberOfLevels()
        {
            for (int level = 0; true; level++)
            {
                int column = currentImporter.GetIndexForHeader((level + 1).ToString());
                if (column == -1)
                {
                    return level;
                }
            }
        }

        private int GetRowForStat(string playerClass, string statString)
        {
            for (int i = 0; i < currentImporter.GetLength(); i++)
            {
                string className = currentImporter.GetCell("Class", i);
                if (className != playerClass) continue;

                string statName = currentImporter.GetCell("Player Details", i);
                if (statName != statString) continue;

                return i;
            }

            return -1;
        }

    }
}