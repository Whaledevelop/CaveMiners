using System.Collections;
using UnityEngine;

public class CharacterState
{
    public CharacterActionData actionData;
    private float stateSkill;
    private CharacterToolsManager toolsManager;
    private Animator animator;

    public CharacterStateData stateData => actionData.stateData;
    public string Name => stateData.stateName;

    public CharacterState(CharacterActionData actionData, float stateSkill, Animator animator, CharacterToolsManager toolsManager)
    {
        this.actionData = actionData;
        this.stateSkill = stateSkill;
        this.toolsManager = toolsManager;
        this.animator = animator;
    }

    public void Start(bool isPrevStateTheSame)
    {
        if (!isPrevStateTheSame)
        {
            if (!string.IsNullOrEmpty(stateData.animatorTriggerStart))
                animator.SetTrigger(stateData.animatorTriggerStart);

            toolsManager.ApplyTool(stateData.toolCode);
        }

        if (stateData.startEvent != null)
        {
            stateData.startEvent.Raise(actionData);
        }
    }

    public void End(bool isNextStateTheSame)
    {
        if (!isNextStateTheSame)
        {
            if (!string.IsNullOrEmpty(stateData.animatorTriggerEnd))
                animator.SetTrigger(stateData.animatorTriggerEnd);

            toolsManager.HideTool(stateData.toolCode);
        }
        if (stateData.endEvent != null)
        {
            stateData.endEvent.Raise(actionData);
        }
    }
}