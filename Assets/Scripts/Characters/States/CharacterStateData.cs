using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStateData", menuName = "States/CharacterStateData")]
public class CharacterStateData : ScriptableObject
{
    public enum StateStage
    {
        None,
        Start,
        End
    }

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
            UpdateView(StateStage.Start, animator, toolsManager);
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
            UpdateView(StateStage.End, animator, toolsManager);
        }
        if (endEvent != null)
        {
            endEvent.Raise(actionData);
        }
        yield break;
    }

    public void UpdateView(StateStage stateStage, Animator animator, CharacterToolsManager toolsManager)
    {
        switch(stateStage)
        {
            case StateStage.Start:
                if (!string.IsNullOrEmpty(animatorTriggerStart))
                    animator.SetTrigger(animatorTriggerStart);
                toolsManager.ApplyTool(toolCode);
                break;
            case StateStage.End:
                if (!string.IsNullOrEmpty(animatorTriggerEnd))
                    animator.SetTrigger(animatorTriggerEnd);
                toolsManager.HideTool(toolCode);
                break;
        }
    }
}