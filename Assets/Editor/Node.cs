using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using RPG.Dialogue;

namespace RPG.Editor.Dialogue
{
    public class Node
    {
        private GUIStyle style = new GUIStyle();
        private Vector2 size = new Vector2(200, 200);
        private ConversationNode source;
        private List<Node> children = new List<Node>();
        Vector2? draggingOffset = null;

        public Node(ConversationNode src)
        {
            style.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            style.border = new RectOffset(12, 12, 12, 12);
            style.padding = new RectOffset(20, 20, 20, 20);
            source = src;
        }

        public void AddChild(Node child)
        {
            children.Add(child);
        }

        public void Draw()
        {
            GUILayout.BeginArea(GetRect(), style);
            var textChanged = GUILayout.TextArea(source.text);
            if (textChanged != source.text)
            {
                source.text = textChanged;
                GUI.changed = true;
            }
            GUILayout.EndArea();

            foreach (var child in children)
            {

                Handles.DrawBezier(GetCentreBottom(), child.GetCentreTop(), GetCentreBottom() + Vector2.up * 100, child.GetCentreTop() + Vector2.down * 100, Color.white, null, 3);
            }
        }

        public void ProcessEvent(Event e)
        {
            switch(e.type)
            {
                case EventType.MouseDown:
                    StartDragging(e);
                    break;
                case EventType.MouseDrag:
                    ExecuteDragging(e);
                    break;
                case EventType.MouseUp:
                    StopDragging(e);
                    break;
            }
        }

        private void StartDragging(Event e)
        {
            if (!GetRect().Contains(e.mousePosition)) return;
            draggingOffset = e.mousePosition - GetRect().position;
        }

        private void ExecuteDragging(Event e)
        {
            if (!draggingOffset.HasValue) return;

            source.position = e.mousePosition - draggingOffset.Value;
            GUI.changed = true;
        }

        private void StopDragging(Event e)
        {
            draggingOffset = null;
        }

        private Rect GetRect()
        {
            return new Rect(source.position, size);
        }

        private Vector2 GetCentreBottom()
        {
            var rect = GetRect();
            return new Vector2(rect.center.x, rect.yMax);
        }

        private Vector2 GetCentreTop()
        {
            var rect = GetRect();
            return new Vector2(rect.center.x, rect.yMin);
        }

    }
}