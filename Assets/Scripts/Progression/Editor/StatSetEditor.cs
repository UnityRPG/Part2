using UnityEngine;
using UnityEditor;
using RPG.Core;
using RPG.Progression;

namespace RPG.ProgressionImport
{
    [CustomEditor(typeof(BaseStatsProgression))]
    public class StatsProgressionEditor : UnityEditor.Editor
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
            var playerStatsProperty = serializedObject.FindProperty("playerStats");

            ImportProperty(playerStatsProperty, "Base Health", "Player", "health");
            ImportProperty(playerStatsProperty, "Base Damage (per hit)", "Player", "damagePerHit");
            ImportProperty(playerStatsProperty, "Typical Ability Damage", "Player", "specialDamage");

            ImportProperty(playerStatsProperty, "XP to level up", "Player", "XPToLevelUp");
            ImportProperty(playerStatsProperty, "Experience per enemy kill", "Player", "XPPerEnemyKill");
        }

        private void ImportEnemies()
        {
            var property = serializedObject.FindProperty("enemyClasses");

            for (int i = 0; i < property.arraySize; i++)
            {
                var enemyClassProperty = property.GetArrayElementAtIndex(i);
                var enemyClassName = enemyClassProperty.FindPropertyRelative("className").stringValue;
                var statsProperty = enemyClassProperty.FindPropertyRelative("enemyStats");

                ImportProperty(statsProperty, "Health", enemyClassName, "health");
                ImportProperty(statsProperty, "Damage per hit", enemyClassName, "damagePerHit");
                ImportProperty(statsProperty, "Hit speed / DPS", enemyClassName, "hitsPerSecond");
                ImportProperty(statsProperty, "Big Hit / special", enemyClassName, "specialDamage");
            }

        }

        private void ImportProperty(SerializedProperty statsProperty, string statString, string characterClass, string propertyName)
        {
            int row = GetRowForStat(characterClass, statString);
            if (row == -1)
            {
                return;
            }

            var statProperty = statsProperty.FindPropertyRelative(propertyName);
            statProperty.arraySize = GetNumberOfLevels();

            for (int i = 0; i < statProperty.arraySize; i++)
            {
                string stat = currentImporter.GetCell((i + 1).ToString(), row);

                var levelProperty = statProperty.GetArrayElementAtIndex(i);

                float floatStat;
                if (float.TryParse(stat, out floatStat))
                {
                    levelProperty.floatValue = floatStat;
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