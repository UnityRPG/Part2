using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using RPG.Dialogue;
using System.Linq;

namespace RPG.Editor.Dialogue
{
    public class Node
    {
        private GUIStyle style = new GUIStyle();
        private Vector2 size = new Vector2(200, 200);
        private ConversationNode nodeModel;
        private Conversation conversationModel;
        Vector2? draggingOffset = null;

        public int index { get; }

        public string id { get; }

        public Node(ConversationNode node, Conversation conversation)
        {
            style.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            style.border = new RectOffset(12, 12, 12, 12);
            style.padding = new RectOffset(20, 20, 20, 20);

            nodeModel = node;
            conversationModel = conversation;
        }

        public void Draw()
        {
            GUILayout.BeginArea(GetRect(), style);
            var textStyle = new GUIStyle(EditorStyles.textArea);
            textStyle.wordWrap = true;
            nodeModel.text = EditorGUILayout.TextArea(nodeModel.text, textStyle);
            GUILayout.EndArea();

            foreach (var childId in nodeModel.children)
            {
                var childModel = conversationModel.GetNodeByUUID(childId);
                if (childModel == null) continue;
                var child = new Node(childModel, conversationModel);
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

            nodeModel.position = e.mousePosition - draggingOffset.Value;
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
            menu.AddItem(new GUIContent("Delete"), false, () => conversationModel.DeleteNode(nodeModel));
            menu.AddItem(new GUIContent("Add connection"), false, null);
            menu.ShowAsContext();
            e.Use();
        }

        private Rect GetRect()
        {
            return new Rect(nodeModel.position, size);
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