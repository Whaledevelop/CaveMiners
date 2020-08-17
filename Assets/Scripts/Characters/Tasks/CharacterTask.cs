using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// Таск - это сумма промежуточных точек, т.е. задача - это сумма подзадача, где подзадача - это выполнение определенного состояния
/// на определенной точке
/// </summary>
public class CharacterTask
{
    public List<PathPoint> taskPoints = new List<PathPoint>();
    private CharacterTaskManager taskManager;
    private int currentTaskPointIndex = 0;
    private CharacterActionState activeState;

    public CharacterTask(List<PathPoint> taskPoints, CharacterTaskManager taskManager)
    {
        this.taskPoints = taskPoints;
        this.taskManager = taskManager;
    }

    /// <summary>
    /// Выполнение всех промежуточных состояний
    /// </summary>
    public IEnumerator Execute()
    {
        while (currentTaskPointIndex < taskPoints.Count)
        {
            PathPoint taskPoint = taskPoints[currentTaskPointIndex];
            activeState = taskManager.ActivateState(taskPoint.state, taskPoint.CellPosition, -taskPoint.AxisToNextCell);
            yield return activeState.Execute();
            currentTaskPointIndex++;
        }
    }

    public IEnumerator Cancel() 
    {
        yield return activeState.Cancel();
    }
}
