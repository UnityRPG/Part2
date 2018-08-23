using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RPG.Editor.Dialogue
{
    public class Node
    {
        private GUIStyle style = new GUIStyle();
        private string text;
        private Rect rect = new Rect();

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

    }
}