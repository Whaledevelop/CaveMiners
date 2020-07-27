using UnityEngine;
using System.Collections;
using UnityEditor;

public class Highlighter  : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Material highlightMaterial;

    private bool highlightMode;

    private Material defaultMaterial;

    void Awake()
    {
        defaultMaterial = sprite.material;
    }

    public void SwapHighlightMode()
    {
        highlightMode = !highlightMode;
        sprite.material = highlightMode ? highlightMaterial : defaultMaterial;
    }
}

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
