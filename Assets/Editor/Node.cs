using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using RPG.Dialogue;
using System.Linq;

namespace RPG.Editor.Dialogue
{
    public class Node : IEquatable<Node>
    {
        private GUIStyle style = new GUIStyle();
        private Vector2 size = new Vector2(200, 200);
        private Vector2 position;
        private string text;
        private ConversationNode source;
        private DialogueEditor dialogueEditor;
        private int index;
        private int[] children;
        Vector2? draggingOffset = null;

        void SetPosition(Vector2 _position)
        {
            source.position = position = _position;
        }

        void SetText(string _text)
        {
            source.text = text = _text;
        }

        public Node(int _index, ConversationNode src, DialogueEditor _dialogueEditor)
        {
            style.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            style.border = new RectOffset(12, 12, 12, 12);
            style.padding = new RectOffset(20, 20, 20, 20);
            source = src;
            text = source.text;
            position = source.position;
            children = (int[])source.children.Clone();
            dialogueEditor = _dialogueEditor;
            index = _index;
        }

        public override bool Equals(object other)
        {
            return Equals(other as Node);
        }

        public bool Equals(Node other)
        {
            bool areEqual = true;
            areEqual &= text == other.text;
            areEqual &= position == other.position;
            areEqual &= children.SequenceEqual(other.children);
            return areEqual;
        }

        public override int GetHashCode()
        {
            int hash = source.text.GetHashCode() ^ source.position.GetHashCode();
            foreach (var child in children)
            {
                hash ^= child.GetHashCode();
            }
            return hash;
        }

        public void Draw()
        {
            GUILayout.BeginArea(GetRect(), style);
            var textStyle = new GUIStyle(EditorStyles.textArea);
            textStyle.wordWrap = true;
            SetText(EditorGUILayout.TextArea(source.text, textStyle));
            GUILayout.EndArea();

            foreach (var childIndex in children)
            {
                var child = dialogueEditor.GetNodeAtIndex(childIndex);
                Handles.DrawBezier(GetCentreBottom(), child.GetCentreTop(), GetCentreBottom() + Vector2.up * 100, child.GetCentreTop() + Vector2.down * 100, Color.white, null, 3);
            }
        }

        public void ProcessEvent(Event e)
        {
            switch(e.type)
            {
                case EventType.MouseDown:
                    if (!GetRect().Contains(e.mousePosition)) break;
                    switch (e.button)
                    {
                        case 0:
                            StartDragging(e);
                            break;
                        case 1:
                            ShowContextMenu(e);
                            break;
                    }
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
            draggingOffset = e.mousePosition - GetRect().position;
            e.Use();
        }

        private void ExecuteDragging(Event e)
        {
            if (!draggingOffset.HasValue) return;

            SetPosition(e.mousePosition - draggingOffset.Value);
            e.Use();
            GUI.changed = true;
        }

        private void StopDragging(Event e)
        {
            if (draggingOffset.HasValue)
            {
                draggingOffset = null;
                e.Use();
            }
        }

        private void ShowContextMenu(Event e)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete"), false, () => dialogueEditor.RemoveNodeAtIndex(index));
            menu.AddItem(new GUIContent("Add connection"), false, null);
            menu.ShowAsContext();
            e.Use();
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