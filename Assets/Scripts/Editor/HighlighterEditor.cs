using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Highlighter)), CanEditMultipleObjects]
[ExecuteInEditMode]
public class HighlighterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("SwapHighlightMode"))
        {
            (target as Highlighter).SwapHighlightMode();
        }
    }
}