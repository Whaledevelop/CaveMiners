using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct StateCharacterActionHandler
{
    public CharacterActionState state;
    public CharacterActionHandler actionHandler;
}

public class CharacterTasksManager : MonoBehaviour
{
    [SerializeField] private CellPositionRequest cellCenterRequest;
    [SerializeField] private CharacterInitialData initialData;
    [SerializeField] private TaskPathfinder taskPathfinder;
    [SerializeField] private Highlighter characterHighlighter;
    [SerializeField] private CharacterState idleState;

    [SerializeField] private CharacterToolsManager toolsManager;
    [SerializeField] private CharacterSkillsManager skillsManager;
    [SerializeField] private Rotator rotator;
    [SerializeField] private Animator animator;

    [SerializeField] private CharacterActionHandler[] actionsHandlers;

    private Dictionary<CharacterTask, IEnumerator> activeTasks = new Dictionary<CharacterTask, IEnumerator>();


    public void ExecuteTask(int taskLayer, Vector2 taskPoint)
    {
        StartCoroutine(ExecuteMainTaskEnumerator(taskLayer, taskPoint));
    }

    public IEnumerator ExecuteMainTaskEnumerator(int taskLayer, Vector2 taskPoint)
    {
        yield return CancelPrevTasks();
        yield return ExecuteTaskEnumerator(taskLayer, taskPoint);
    }

    public IEnumerator ExecuteTaskEnumerator(int taskLayer, Vector2 taskPoint)
    {
        cellCenterRequest.MakeRequest(new ParamsObject(taskPoint), out taskPoint);
        List<CharacterTaskPoint> taskStatesPoints = taskPathfinder.FindPath(transform.position, taskPoint, taskLayer);
        if (taskStatesPoints.Count > 0)
        {
            CharacterTask task = new CharacterTask(taskStatesPoints, this, toolsManager, skillsManager, animator, rotator, actionsHandlers);
            IEnumerator taskCoroutine = task.Execute();
            activeTasks.Add(task, taskCoroutine);

            yield return taskCoroutine;

            activeTasks.Remove(task);
        }
        else
        {
            Debug.Log("Task with no states");
        }
    }

    public IEnumerator CancelPrevTasks()
    {
        foreach(KeyValuePair<CharacterTask, IEnumerator> task in activeTasks)
        {
            StopCoroutine(task.Value);
            yield return task.Key.Cancel();
        }
        activeTasks.Clear();
    }

    public IEnumerator ExecuteState(CharacterActionState state, Vector2 endPosition, Vector2 actionDirection)
    {
        CharacterAction actionData = new CharacterAction(this, skillsManager, state, transform.position, endPosition, actionDirection);

        CharacterActionHandler actionHandler = actionsHandlers.FirstOrDefault(handler => handler.HandledState == state);

        CharacterActionState activeState = ScriptableObject.Instantiate(state);
        activeState.InitInstance(animator, toolsManager, rotator, actionData, actionHandler);

        yield return activeState.Execute();
    }

    public override string ToString() => initialData.name;

    public void OnDrawGizmos()
    {
        if (activeTasks != null)
        {
            foreach(KeyValuePair<CharacterTask, IEnumerator> task in activeTasks)
            {
                foreach (CharacterTaskPoint point in task.Key.taskPoints)
                {
                    Gizmos.color = point.stateData.gizmosColor;
                    Gizmos.DrawSphere(point.CellPosition, 0.1f);
                    Gizmos.DrawLine(point.CellPosition, point.NextCellToCharacterPosition);
                }
            }
        }
    }
}
