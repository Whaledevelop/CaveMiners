using UnityEngine;



[CreateAssetMenu(fileName = "CharacterStateData", menuName = "ScriptableObjects/CharacterStateData")]
public class CharacterStateData : ScriptableObject
{
    public string stateName;
 
    public ToolCode toolCode;
    public LayerMask actionLayerMask;

    public int actionPriority;

    public Color gizmosColor;
    public RotationMode rotationMode;

    public string animatorTriggerStart;
    public string animatorTriggerEnd;
    public CharacterActionGameEvent startEvent;
    public CharacterActionGameEvent endEvent;
    
    // Перенести отсюда
    public bool CompareActionMaskWithLayer(int layer)
    {
        return (int)Mathf.Log(actionLayerMask.value, 2) == layer;
    }
}