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
        Dictionary<string, Node> nodeViews = new Dictionary<string, Node>();
        Conversation currentSelection;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        void OnEnable()
        {
            Selection.selectionChanged += SelectionChanged;

            SelectionChanged();
        }

        void SelectionChanged()
        {
            Debug.Log("SelectionChanged");
            currentSelection = Selection.activeObject as Conversation;
            currentSelection.onValidated += OnModelUpdated;

            OnModelUpdated();
        }

        void OnModelUpdated()
        {
            nodeViews = ReloadNodes();
            Repaint();
        }

        void OnGUI()
        {
            if (!currentSelection) return;

            scrollPosition = GUI.BeginScrollView(new Rect(Vector2.zero, position.size), scrollPosition, Canvas);

            foreach (KeyValuePair<string, Node> nodeViewPair in nodeViews)
            {
                nodeViewPair.Value.Draw();
                nodeViewPair.Value.ProcessEvent(Event.current);
            }
    
            GUI.EndScrollView();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(currentSelection);
                Repaint();
            }
        }

        public Node GetNodeAtID(string id)
        {
            Node ret;
            return nodeViews.TryGetValue(id, out ret) ? ret : null;
        }

        public void RemoveNodeAtID(string id)
        {
            currentSelection.nodes.RemoveAt(nodeViews[id].index);
            nodeViews.Remove(id);
            GUI.changed = true;
        }

        private Dictionary<string, Node> ReloadNodes()
        {
            var nodeViews = new Dictionary<string, Node>();

            if (!currentSelection) return nodeViews;

            foreach (var node in currentSelection.nodes)
            {
                nodeViews[node.UUID] = new Node(nodeViews.Count, node, this);
            }

            return nodeViews;
        }

    }
}