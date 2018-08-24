using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RPG.Dialogue;
using System.Linq;

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

        void OnInspectorUpdate()
        {
            currentSelection = Selection.activeObject as Conversation;
            var newNodeViews = ReloadNodes();
            if (!newNodeViews.SequenceEqual(nodeViews))
            {
                nodeViews = newNodeViews;
                Debug.Log("repaint");
                Repaint();
            }
        }

        void OnGUI()
        {
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

        public Node GetNodeAtIndex(int index)
        {
            return nodeViews[index];
        }

        public void RemoveNodeAtIndex(int index)
        {
            nodeViews.RemoveAt(index);
            currentSelection.nodes.RemoveAt(index);
            GUI.changed = true;
        }

        private List<Node> ReloadNodes()
        {
            var nodeViews = new List<Node>();

            if (!currentSelection) return nodeViews;

            foreach (var node in currentSelection.nodes)
            {
                nodeViews.Add(new Node(nodeViews.Count, node, this));
            }

            return nodeViews;
        }

    }
}