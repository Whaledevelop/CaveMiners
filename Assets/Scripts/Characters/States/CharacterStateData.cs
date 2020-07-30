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

    [Header("Обновляется ли внешний вид при нескольких состояниях подряд")]
    public bool isViewUpdatableIfSame;

    public string animatorTriggerStart;
    public string animatorTriggerEnd;
    public CharacterActionGameEvent startEvent;
    public CharacterActionGameEvent endEvent;

    public virtual IEnumerator Execute(bool isPrevStateTheSame, CharacterActionData actionData, Animator animator, CharacterToolsManager toolsManager, Rotator rotator)
    {
        if (isViewUpdatableIfSame || (!isViewUpdatableIfSame && !isPrevStateTheSame))
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
        if (isViewUpdatableIfSame || (!isViewUpdatableIfSame && !isNextStateTheSame))
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