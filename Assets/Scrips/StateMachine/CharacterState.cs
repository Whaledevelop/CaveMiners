using UnityEngine;

public class CharacterState
{
    public CharacterStateData stateData;

    public Animator animator;

    public MinerTools toolManager;

    public CharacterState(CharacterStateData stateData, Animator animator, MinerTools toolManager)
    {
        this.stateData = stateData;
        this.animator = animator;
        this.toolManager = toolManager;
    }

    public void OnStart()
    {
        if (!string.IsNullOrEmpty(stateData.animatorTriggerStart))
            animator.SetTrigger(stateData.animatorTriggerStart);
        else if (!string.IsNullOrEmpty(stateData.animatorTriggerEnd))
            toolManager.ApplyTool(stateData.toolStringName);

        if (stateData.StartEvent != null)
            stateData.StartEvent.Raise();
    }
    public void OnEnd()
    {
        if (!string.IsNullOrEmpty(stateData.animatorTriggerEnd))
            animator.SetTrigger(stateData.animatorTriggerEnd);
        else if (!string.IsNullOrEmpty(stateData.animatorTriggerEnd))
            toolManager.HideTool(stateData.toolStringName);

        if (stateData.EndEvent != null)
            stateData.EndEvent.Raise();
    }
}