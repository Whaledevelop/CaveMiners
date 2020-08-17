using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Обработчик копания
/// </summary>
public class DigActionHandler : CharacterActionHandler
{
    [SerializeField] private DigCharacterState digState;

    private bool isDigging;

    private CharacterAction actionData;

    public override CharacterActionState HandledState => digState;

    /// <summary>
    /// Выполнение сводится к вызову раз в определенный интервал события, которое обрабатывается скриптами тайлов (урон),
    /// до тех пор, пока не вызовется событие окончания действия, которое обработается в OnTileWorkedOut
    /// </summary>
    public override IEnumerator Execute(CharacterAction actionData)
    {
        isDigging = true;
        this.actionData = actionData;
        while (isDigging)
        {
            yield return new WaitForSeconds(digState.iterationsInterval);
            digState.iterationEvent.Raise(actionData);
            actionData.LearnSkill();
        }
        this.actionData = null;
    }

    /// <summary>
    /// Обработчик не сам решает, когда ему закончить выполнение, он ждет события
    /// </summary>
    public void OnTileWorkedOut(CharacterAction tileWorkedOutActionData)
    {
        if (actionData != null)
        {
            bool currentState = tileWorkedOutActionData.stateData == HandledState;
            bool currentTile = tileWorkedOutActionData.endPosition == actionData.endPosition;
            if (currentState && currentTile)
            {
                isDigging = false;                
            }
        }       
    }

    /// <summary>
    /// Прервать выполнения, не дождавшись раскопки
    /// </summary>
    public override IEnumerator Cancel()
    {
        isDigging = false;
        yield break;
    }
}
