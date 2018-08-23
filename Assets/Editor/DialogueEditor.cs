using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RPG.Dialogue;

namespace RPG.Editor.Dialogue
{
    public class DialogueEditor : EditorWindow
    {

        Vector2 scrollPosition = Vector2.zero;
        Rect Canvas = new Rect(0, 0, 4000, 4000);
        List<Node> nodeViews = new List<Node>();
        Conversation currentSelection;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        void OnGUI()
        {
            var conversation = Selection.activeObject as Conversation;
            if (currentSelection != conversation)
            {
                Debug.Log("Changed");
                currentSelection = conversation;
                GUI.changed = true;
                ReloadNodes();
            }
            if (!currentSelection) return;

            scrollPosition = GUI.BeginScrollView(new Rect(Vector2.zero, position.size), scrollPosition, Canvas);

            foreach (var nodeView in nodeViews)
            {
                nodeView.Draw();
                nodeView.ProcessEvent(Event.current);
            }
    
            GUI.EndScrollView();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(currentSelection);
                Repaint();
            }
        }

        private void ReloadNodes()
        {
            if (!currentSelection) return;

            nodeViews.Clear();
            foreach (var node in currentSelection.nodes)
            {
                nodeViews.Add(new Node(node));
            }
        }
    }
}