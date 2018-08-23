using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RPG.Editor.Dialogue
{
    public class DialogueEditor : EditorWindow
    {

        Vector2 scrollPosition = Vector2.zero;
        Rect Canvas = new Rect(0, 0, 4000, 4000);
        List<Node> nodes = new List<Node>();

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        void OnEnable()
        {
            nodes.Clear();
            nodes.Add(new Node(new Vector2(100, 200)));
            nodes.Add(new Node(new Vector2(400, 200)));
        }

        void OnGUI()
        {
            scrollPosition = GUI.BeginScrollView(new Rect(Vector2.zero, position.size), scrollPosition, Canvas);

            foreach (var node in nodes)
            {
                node.Draw();
            }
    
            GUI.EndScrollView();
        }
    }
}