using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenerateInAreaRule)), CanEditMultipleObjects]
public class GenerateInAreaRuleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GenerateInAreaRule rule = target as GenerateInAreaRule;

        EditorGUILayout.Space();
        EditorGUILayout.PrefixLabel("X area, from none to full level");
        EditorGUILayout.MinMaxSlider(ref rule.xArea.from, ref rule.xArea.to, 0, 1);

        EditorGUILayout.PrefixLabel("Y area, from none to full level");
        EditorGUILayout.MinMaxSlider(ref rule.yArea.from, ref rule.yArea.to, 0, 1);

    }
}