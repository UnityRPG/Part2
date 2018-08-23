using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class DialogueEditor : EditorWindow {

    Vector2 scrollPosition = Vector2.zero;
    Rect Canvas = new Rect(0,0, 4000, 4000);

    [MenuItem("Window/Dialogue Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
    }

    void OnGUI()
    {
        scrollPosition = GUI.BeginScrollView(new Rect(Vector2.zero, position.size), scrollPosition, Canvas);

        GUI.EndScrollView();
    }
}
