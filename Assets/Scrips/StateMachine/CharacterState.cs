using UnityEngine;

public class CharacterState
{
    public CharacterStateData stateData;

    public Animator animator;

    public MinerTools toolManager;

    private ParamsObject requestResolveParams;

    public string Name => stateData.stateName;

    public CharacterState(CharacterStateData stateData, Animator animator, MinerTools toolManager)
    {
        this.stateData = stateData;
        this.animator = animator;
        this.toolManager = toolManager;
    }

    public virtual bool CheckIfStartAvailable(Vector2 position, Vector2 direction)
    {
        if (stateData.startRequest != null)
        {
            requestResolveParams = stateData.startRequest.MakeRequest(position, direction);
            return requestResolveParams != null;
        }
        else
            return true;
    }

    public virtual void OnStartNotAvailable(){}

    public virtual void OnStart()
    {
        if (!string.IsNullOrEmpty(stateData.animatorTriggerStart))
            animator.SetTrigger(stateData.animatorTriggerStart);

        toolManager.ApplyTool(stateData.toolCode);

        if (stateData.startEvent != null)
        {
            if (stateData.areResolveParamsNeededInEvent)
                stateData.startEvent.Raise(requestResolveParams);
            else
                stateData.startEvent.Raise();
        }
            
    }
    public virtual void OnEnd()
    {
        if (!string.IsNullOrEmpty(stateData.animatorTriggerEnd))
            animator.SetTrigger(stateData.animatorTriggerEnd);

        toolManager.HideTool(stateData.toolCode);

        if (stateData.endEvent != null)
            stateData.endEvent.Raise();
    }
}