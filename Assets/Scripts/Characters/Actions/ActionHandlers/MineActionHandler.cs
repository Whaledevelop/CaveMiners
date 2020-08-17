using UnityEngine;
using System.Collections;

/// <summary>
/// Обработчик добычи
/// </summary>
public class MineActionHandler : CharacterActionHandler
{
    [SerializeField] private MineCharacterState mineState;

    private bool isMining;

    private int iterationCount;

    private CharacterAction actionData;

    public override CharacterActionState HandledState => mineState;


    /// <summary>
    /// Выполнение сводится к вызову раз в определенный интервал события, которое обрабатывается скриптами тайлов (урон),
    /// до тех пор, пока не вызовется событие окончания действия, которое обработается в OnTileWorkedOut или пока не
    /// событие не вызовется определенное количество раз
    /// </summary>
    public override IEnumerator Execute(CharacterAction actionData)
    {
        isMining = true;
        this.actionData = actionData;
        while (isMining && iterationCount < mineState.maxIterations)
        {
            yield return new WaitForSeconds(mineState.iterationsInterval);
            mineState.iterationEvent.Raise(actionData);
            actionData.LearnSkill();
            iterationCount++;
        }
        EndIterations();
    }

    /// <summary>
    /// Клетка разработана - нужно прервать выполнение
    /// </summary>
    public void OnTileWorkedOut(CharacterAction tileWorkedOutActionData)
    {
        if (actionData != null)
        {
            if (tileWorkedOutActionData.stateData == HandledState && tileWorkedOutActionData.endPosition == actionData.endPosition)
            {
                EndIterations();
            }
        }
    }

    /// <summary>
    /// Прервать выполнения, не дождавшись конца добычи
    /// </summary>
    public override IEnumerator Cancel()
    {
        EndIterations();
        yield break;
    }

    private void EndIterations()
    {
        isMining = false;
        iterationCount = 0;
    }
}
