using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RPG.Dialogue;
using System.Linq;
using NodeEditorFramework.Utilities;

namespace RPG.Editor.Dialogue
{
    public class DialogueEditor : EditorWindow
    {

        Vector2 scrollPosition = Vector2.zero;
        Rect Canvas = new Rect(0, 0, 4000, 4000);
        List<Node> nodeViews = new List<Node>();
        Conversation currentSelection;
        Vector2 lastMousePosition = new Vector2(0, 0);
        float zoomLevel = 0.8f;

        public Node linkingNode { get; set; }

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
            if (currentSelection == null) return;

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

            var screenSize = new Rect(0, 0, Screen.width, Screen.height);
            GUI.EndClip();
            GUI.EndClip();
            GUI.BeginGroup(new Rect(0, 0, screenSize.width, (Screen.height - 100) * zoomLevel));
            var mat = GUI.matrix;
            GUIUtility.ScaleAroundPivot(new Vector2(zoomLevel, zoomLevel), new Vector2(0, 0));
            //GUIScaleUtility.BeginScale(ref Canvas, new Vector2(0,0), zoomLevel, true, true);

            foreach (Node nodeView in nodeViews)
            {
                nodeView.Draw(zoomLevel);
                nodeView.ProcessEvent(Event.current);
            }
            //GUIScaleUtility.EndScale();
            GUI.matrix = mat;
            GUI.EndGroup();
            GUI.BeginClip(new Rect(0, 23, Screen.width, Screen.height - 23));
            GUI.BeginClip(new Rect(0, 23, Screen.width, Screen.height - 23));
            GUI.EndScrollView();

            lastMousePosition =  Event.current.mousePosition + scrollPosition;
            ProcessEvent(Event.current);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(currentSelection);
                Repaint();
            }
        }

        void ProcessEvent(Event e)
        {
            switch (e.type)
            {
                case EventType.ContextClick:
                    ShowContextMenu();
                    e.Use();
                    break;
            }
        }

        void ShowContextMenu()
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add node"), false, () => currentSelection?.AddNode(lastMousePosition));
            menu.ShowAsContext();
        }

        private void ReloadNodes()
        {
            nodeViews.Clear();

            if (!currentSelection) return;

            foreach (var node in currentSelection.nodes)
            {
                nodeViews.Add(new Node(node, currentSelection, this));
            }
        }

    }
}