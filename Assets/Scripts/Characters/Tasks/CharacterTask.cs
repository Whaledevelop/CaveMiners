using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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
            Vector2 axisToCharacter = taskPoint.PointPosition - taskPoint.closerToCharacterPointPosition;
            activeState = taskManager.ActivateState(taskPoint.state, taskPoint.PointPosition, axisToCharacter);
            yield return activeState.Execute();
            currentTaskPointIndex++;
        }
    }

    public IEnumerator Cancel() 
    {
        yield return activeState.Cancel();
    }
}
