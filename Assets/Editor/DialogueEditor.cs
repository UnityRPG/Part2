using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class DialogueEditor : EditorWindow {

    Vector2 scrollPosition = Vector2.zero;
    Rect Canvas = Rect.zero;

    [MenuItem("Window/Dialogue Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
    }

    void OnGUI()
    {
        Canvas.size = new Vector2(4000, 4000);
        scrollPosition = GUI.BeginScrollView(new Rect(0, 0, position.x, position.y), scrollPosition, Canvas);
        GUI.Box(Canvas, "Hi");
        GUI.EndScrollView();
    }
}
