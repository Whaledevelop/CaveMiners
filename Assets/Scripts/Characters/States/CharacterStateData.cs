using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStateData", menuName = "States/CharacterStateData")]
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

    public virtual IEnumerator Execute(bool isPrevStateTheSame, CharacterActionData actionData, Animator animator, CharacterToolsManager toolsManager, Rotator rotator)
    {
        if (!isPrevStateTheSame)
        {
            if (!string.IsNullOrEmpty(animatorTriggerStart))
                animator.SetTrigger(animatorTriggerStart);

            toolsManager.ApplyTool(toolCode);
        }

        rotator.Rotate(actionData.actionDirection, rotationMode);

        if (startEvent != null)
        {
            startEvent.Raise(actionData);
        }
        yield break;
    }

    public virtual IEnumerator End(bool isNextStateTheSame, CharacterActionData actionData, Animator animator, CharacterToolsManager toolsManager, Rotator rotator)
    {
        Debug.Log("End movement");
        if (!isNextStateTheSame)
        {
            if (!string.IsNullOrEmpty(animatorTriggerEnd))
                animator.SetTrigger(animatorTriggerEnd);

            toolsManager.HideTool(toolCode);
        }
        if (endEvent != null)
        {
            endEvent.Raise(actionData);
        }
        yield break;
    }
}