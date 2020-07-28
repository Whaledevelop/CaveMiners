﻿using System.Collections;
using UnityEngine;

public class CharacterState
{
    public CharacterActionData actionData;
    private CharacterToolsManager toolsManager;
    private Animator animator;
    private Rotator rotator;

    public CharacterStateData stateData => actionData.stateData;
    public string Name => stateData.stateName;

    public bool isExecuting;

    public CharacterState(CharacterActionData actionData, Animator animator, CharacterToolsManager toolsManager, Rotator rotator)
    {
        this.actionData = actionData;
        this.toolsManager = toolsManager;
        this.animator = animator;
        this.rotator = rotator;
    }

    public IEnumerator Start(bool isPrevStateTheSame)
    {
        if (!isPrevStateTheSame)
        {
            if (!string.IsNullOrEmpty(stateData.animatorTriggerStart))
                animator.SetTrigger(stateData.animatorTriggerStart);

            toolsManager.ApplyTool(stateData.toolCode);
        }

        rotator.Rotate(actionData.actionDirection, stateData.rotationMode);

        if (stateData.startEvent != null)
        {
            stateData.startEvent.Raise(actionData);
        }
        if (stateData.iterationEvent != null)
        {
            isExecuting = true;
            while (isExecuting)
            {
                yield return new WaitForSeconds(stateData.iterationsInterval);
                stateData.iterationEvent.Raise(actionData);  
            }            
        }
        else
        {
            yield break;
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