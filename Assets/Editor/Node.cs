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
        private List<Node> children = new List<Node>();
        Vector2? draggingOffset = null;

        void SetPosition(Vector2 _position)
        {
            source.position = position = _position;
        }

        void SetText(string _text)
        {
            source.text = text = _text;
        }

        public Node(ConversationNode src)
        {
            style.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            style.border = new RectOffset(12, 12, 12, 12);
            style.padding = new RectOffset(20, 20, 20, 20);
            source = src;
            text = source.text;
            position = source.position;
        }

        public override bool Equals(object other)
        {
            Debug.Log("Checking equal");
            return Equals(other as Node);
        }

        public bool Equals(Node other)
        {
            bool areEqual = true;
            areEqual &= text == other.text;
            areEqual &= position == other.position;
            areEqual &= children.SequenceEqual(other.children);
            Debug.Log("Checking equal node" + areEqual);
            return areEqual;
        }

        public override int GetHashCode()
        {
            Debug.Log("Checking equal hash");

            int hash = source.text.GetHashCode() ^ source.position.GetHashCode();
            foreach (var child in children)
            {
                hash ^= child.GetHashCode();
            }
            return hash;
        }

        public void AddChild(Node child)
        {
            children.Add(child);
        }

        public void Draw()
        {
            GUILayout.BeginArea(GetRect(), style);
            SetText(GUILayout.TextArea(source.text));
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