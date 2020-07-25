using System.Collections;
using UnityEngine;

public class CharacterState
{
    public CharacterActionData actionData;

    private Animator animator;

    private CharacterToolsManager toolsManager;

    private float stateSkill;

    public CharacterStateData stateData => actionData.stateData;
    public string Name => stateData.stateName;

    public CharacterState(CharacterActionData actionData, float stateSkill, Animator animator, CharacterToolsManager toolManager)
    {
        this.actionData = actionData;
        this.stateSkill = stateSkill;
        this.animator = animator;
        this.toolsManager = toolManager;
    }

    public void OnStart()
    {
        if (!string.IsNullOrEmpty(stateData.animatorTriggerStart))
            animator.SetTrigger(stateData.animatorTriggerStart);

        toolsManager.ApplyTool(stateData.toolCode);

        if (stateData.startEvent != null)
        {
            stateData.startEvent.Raise(actionData);
        }
    }

    public void OnEnd()
    {
        if (!string.IsNullOrEmpty(stateData.animatorTriggerEnd))
            animator.SetTrigger(stateData.animatorTriggerEnd);

        toolsManager.HideTool(stateData.toolCode);

        if (stateData.endEvent != null)
        {
            stateData.endEvent.Raise(actionData);
        }
    }
}