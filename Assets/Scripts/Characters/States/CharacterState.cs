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

    private int iterationsCounter;

    public CharacterState(CharacterActionData actionData, Animator animator, CharacterToolsManager toolsManager, Rotator rotator)
    {
        this.actionData = actionData;
        this.toolsManager = toolsManager;
        this.animator = animator;
        this.rotator = rotator;
    }

    public IEnumerator Execute(bool isPrevStateTheSame = false)
    {
        this.isPrevStateTheSame = isPrevStateTheSame;
        isExecuting = true;
        yield return stateData.Execute(isPrevStateTheSame, actionData, animator, toolsManager, rotator);
        if (stateData is IterativeStateData)
        {
            yield return StartIterations();
        }
        isExecuting = false;
    }

    private IEnumerator StartIterations()
    {
        IterativeStateData iterativeState = stateData as IterativeStateData;
        if (iterativeState.iterationEvent != null)
        {
            while (DefineIfIterationsEnd(iterativeState))
            {
                yield return Iterate(actionData);
            }
            if (iterativeState.executionCondition == EndExecutionCondition.IterationsCount && iterationsCounter > iterativeState.maxIterationsCount)
            {
                
                actionData.OnExecute(EndExecutionCondition.IterationsCount);
            }
        }
        else
        {
            yield break;
        }
    }

    private bool DefineIfIterationsEnd(IterativeStateData iterativeState)
    {
        bool isTilTheExecution = iterativeState.executionCondition == EndExecutionCondition.Executed;
        bool isWhileIterationsCount = iterativeState.executionCondition == EndExecutionCondition.IterationsCount &&
            iterationsCounter <= iterativeState.maxIterationsCount;
        return isTilTheExecution || isWhileIterationsCount;
    }

    public virtual IEnumerator Iterate(CharacterActionData actionData)
    {
        IterativeStateData iterativeState = stateData as IterativeStateData;
        yield return new WaitForSeconds(iterativeState.iterationsInterval);
        iterativeState.iterationEvent.Raise(actionData);
        if (iterativeState.executionCondition == EndExecutionCondition.IterationsCount)
            iterationsCounter++;
    }

    public IEnumerator End(bool isNextStateTheSame = false)
    {
        actionData.taskManager.StopCoroutine(Execute(isPrevStateTheSame));

        yield return stateData.End(isNextStateTheSame, actionData, animator, toolsManager, rotator);
    }
}