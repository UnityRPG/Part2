using UnityEngine;
using UnityEditor;

namespace RPG.Core.Stats
{
    [CustomEditor(typeof(StatSet))]
    public class StatSetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Import CSV"))
            {
                string path = EditorUtility.OpenFilePanel("Import CSV", "", "csv");
                var importer = new CSVImporter(path);
                importer.Load();
                Debug.Log(importer.GetCell("1", 10));
            }

            DrawDefaultInspector();
        }
    }
}