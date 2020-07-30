using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "IterativeStateData", menuName = "States/IterativeStateData")]
public class IterativeStateData : CharacterStateData
{
    public enum EndExecutionCondition
    {
        Executed,
        IterationsCountOrExecuted
    }

    public CharacterActionGameEvent iterationEvent;
    public float iterationsInterval = 1;

    public EndExecutionCondition executionCondition;

    public int maxIterationsCount;

    [NonSerialized] private int iterationsCounter;

    public override IEnumerator Execute(bool isPrevStateTheSame, CharacterActionData actionData, Animator animator, CharacterToolsManager toolsManager, Rotator rotator)
    {
        yield return base.Execute(isPrevStateTheSame, actionData, animator, toolsManager, rotator);
        if (iterationEvent != null)
        {
            while (executionCondition == EndExecutionCondition.Executed || (executionCondition == EndExecutionCondition.IterationsCountOrExecuted && iterationsCounter <= maxIterationsCount))
            {
                yield return Iterate(actionData);
            }
            yield return End(false, actionData, animator, toolsManager, rotator);
        }
        else
        {
            yield break;
        }
    }

    public virtual IEnumerator Iterate(CharacterActionData actionData)
    {
        Debug.Log(iterationsCounter);
        yield return new WaitForSeconds(iterationsInterval);
        iterationEvent.Raise(actionData);
        if (executionCondition == EndExecutionCondition.IterationsCountOrExecuted)
            iterationsCounter++;
    }
}