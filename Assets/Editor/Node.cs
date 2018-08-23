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
        private string text;
        private Rect rect = new Rect();
        Vector2? draggingOffset = null;

        public Node(Vector2 position)
        {
            style.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            style.border = new RectOffset(12, 12, 12, 12);
            style.padding = new RectOffset(20, 20, 20, 20);
            rect.size = new Vector2(200, 200);
            rect.position = position;
        }

        public void Draw()
        {
            GUILayout.BeginArea(rect, style);
            text = GUILayout.TextArea(text);
            GUILayout.EndArea();
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
            if (!rect.Contains(e.mousePosition)) return;
            draggingOffset = e.mousePosition - rect.position;
        }

        private void ExecuteDragging(Event e)
        {
            if (!draggingOffset.HasValue) return;

            rect.position = e.mousePosition - draggingOffset.Value;
            GUI.changed = true;
        }

        private void StopDragging(Event e)
        {
            draggingOffset = null;
        }

    }
}