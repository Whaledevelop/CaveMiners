using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "CharacterIterativeActionState", menuName = "States/CharacterIterativeActionState")]
public class CharacterIterativeActionState : CharacterActionState
{
    // Общие данные
    public CharacterActionGameEvent iterationEvent;
    public float iterationsRange = 1;

    public EndExecutionCondition executionCondition;

    public int maxIterationsCount;

    // Данные экземпляров
    [NonSerialized] protected int iterationsCounter;

    public override IEnumerator Start()
    {
        yield return base.Start();
        yield return StartIterations();
    }

    public override IEnumerator End()
    {
        yield return base.End();
        // Остановить итерации
    }

    private IEnumerator StartIterations()
    {
        if (iterationEvent != null)
        {
            while (DefineIfIterationsEnd())
            {
                yield return Iterate(actionData);
            }
            if (executionCondition == EndExecutionCondition.IterationsCount && iterationsCounter > maxIterationsCount)
            {
                actionData.OnExecute(EndExecutionCondition.IterationsCount);
            }
        }
        else
        {
            yield break;
        }
    }

    private bool DefineIfIterationsEnd()
    {
        bool isTilTheExecution = executionCondition == EndExecutionCondition.Executed;
        bool isWhileIterationsCount = executionCondition == EndExecutionCondition.IterationsCount &&
            iterationsCounter <= maxIterationsCount;
        return isTilTheExecution || isWhileIterationsCount;
    }

    public virtual IEnumerator Iterate(CharacterActionData actionData)
    {
        yield return new WaitForSeconds(iterationsRange);
        iterationEvent.Raise(actionData);
        if (executionCondition == EndExecutionCondition.IterationsCount)
            iterationsCounter++;
    }
}