using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Обработчик выполнения задания для персонажа
/// </summary>
public class CharacterTaskManager : CharacterManager
{
    [Header("Реквест для определения центра клетки")]
    [SerializeField] private CellPositionRequest cellCenterRequest;
    [Header("Определитель пути до цели")]
    [SerializeField] private TaskPathfinder taskPathfinder;
    [Header("Обработчики действий на клетках")]
    [SerializeField] private CharacterActionHandler[] actionsHandlers;

    [SerializeField] private Rotator rotator;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterSkillsManager skillsManager;
    [SerializeField] private CharacterToolsManager toolsManager;

    private CharacterTask activeTask;
    private IEnumerator taskCoroutine;

    public void ExecuteTask(int taskLayer, Vector2 taskPoint)
    {
        if (taskCoroutine != null)
            StopCoroutine(taskCoroutine);

        taskCoroutine = ExecuteTaskEnumerator(taskLayer, taskPoint);
        StartCoroutine(taskCoroutine);
    }
    private IEnumerator ExecuteTaskEnumerator(int taskLayer, Vector2 taskPoint)
    {
        if (activeTask != null)
            yield return activeTask.Cancel();
        activeTask = ActivateTask(taskLayer, taskPoint);
        if (activeTask != null)
            yield return activeTask.Execute();
    }

    /// <summary>
    /// Вынесено в отдельный публичный метод, чтобы можно было создавать таски вне TaskManager - для внутренних тасков
    /// </summary>
    public CharacterTask ActivateTask(int taskLayer, Vector2 taskPoint)
    {
        cellCenterRequest.MakeRequest(new ParamsObject(taskPoint), out taskPoint);
        List<PathPoint> taskPathPoints = taskPathfinder.FindPath(transform.position, taskPoint, taskLayer);
        if (taskPathPoints.Count > 0)
        {
            return new CharacterTask(taskPathPoints, this);
        }
        else
        {
            return null;
        }
    }

    public CharacterActionState ActivateState(CharacterActionState state, Vector2 endPosition, Vector2 actionDirection)
    {
        // Все необходимые данные для выполнения действия состояния
        CharacterAction actionData = new CharacterAction(this, skillsManager, state, transform.position, endPosition, actionDirection);
        // Обработчик действия на "местности"
        CharacterActionHandler actionHandler = actionsHandlers.FirstOrDefault(handler => handler.HandledState == state);
        // Создаем экземпляр состояния из шаблона
        CharacterActionState activeState = ScriptableObject.Instantiate(state);
        activeState.InitInstance(animator, toolsManager, rotator, actionData, actionHandler);
        return activeState;        
    }

    public override string ToString() => character.ToString();

    public void OnDrawGizmos()
    {
        if (activeTask != null)
        {
            foreach (PathPoint point in activeTask.taskPoints)
            {
                Gizmos.color = point.state.gizmosColor;
                Gizmos.DrawLine(point.closerToCharacterPointPosition, point.PointPosition);
                Gizmos.DrawSphere(point.PointPosition, 0.1f);
            }
        }
    }
}
