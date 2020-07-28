using System.Collections;
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

    private bool isPrevStateTheSame;

    public CharacterState(CharacterActionData actionData, Animator animator, CharacterToolsManager toolsManager, Rotator rotator)
    {
        this.actionData = actionData;
        this.toolsManager = toolsManager;
        this.animator = animator;
        this.rotator = rotator;
    }

    public IEnumerator Execute(bool isPrevStateTheSame)
    {
        this.isPrevStateTheSame = isPrevStateTheSame;
        isExecuting = true;
        yield return stateData.Execute(isPrevStateTheSame, actionData, animator, toolsManager, rotator);
        isExecuting = false;
    }

    public IEnumerator End(bool isNextStateTheSame)
    {
        actionData.taskManager.StopCoroutine(Execute(isPrevStateTheSame));
        yield return stateData.End(isNextStateTheSame, actionData, animator, toolsManager, rotator);
    }
}