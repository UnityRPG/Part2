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

        void OnEnable()
        {
            Selection.selectionChanged += SelectionChanged;

            SelectionChanged();
        }

        void SelectionChanged()
        {
            currentSelection = Selection.activeObject as Conversation;
            currentSelection.onValidated += OnModelUpdated;

            OnModelUpdated();
        }

        void OnModelUpdated()
        {
            ReloadNodes();
            Repaint();
        }

        void OnGUI()
        {
            if (!currentSelection) return;

            scrollPosition = GUI.BeginScrollView(new Rect(Vector2.zero, position.size), scrollPosition, Canvas);

            foreach (Node nodeView in nodeViews)
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
            nodeViews.Clear();

            if (!currentSelection) return;

            foreach (var node in currentSelection.nodes)
            {
                nodeViews.Add(new Node(node, currentSelection));
            }
        }

    }
}