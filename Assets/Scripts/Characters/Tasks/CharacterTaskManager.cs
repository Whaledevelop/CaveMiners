using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterTaskManager : CharacterManager
{
    [SerializeField] private CellPositionRequest cellCenterRequest;
    [SerializeField] private TaskPathfinder taskPathfinder;
    [SerializeField] private CharacterState idleState;
    [SerializeField] private Rotator rotator;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterSkillsManager skillsManager;
    [SerializeField] private CharacterToolsManager toolsManager;
    [SerializeField] private CharacterActionHandler[] actionsHandlers;

    private CharacterTask activeTask;
    private IEnumerator taskCoroutine;

    public void ExecuteTask(int taskLayer, Vector2 taskPoint)
    {
        if (taskCoroutine != null)
            StopCoroutine(taskCoroutine);

        taskCoroutine = ExecuteTaskEnumerator(taskLayer, taskPoint);
        StartCoroutine(taskCoroutine);
    }

    public IEnumerator ExecuteTaskEnumerator(int taskLayer, Vector2 taskPoint)
    {
        if (activeTask != null)
            yield return activeTask.Cancel();
        activeTask = ActivateTask(taskLayer, taskPoint);
        yield return activeTask.Execute();
    }

    public CharacterTask ActivateTask(int taskLayer, Vector2 taskPoint)
    {
        cellCenterRequest.MakeRequest(new ParamsObject(taskPoint), out taskPoint);
        List<PathPoint> taskStatesPoints = taskPathfinder.FindPath(transform.position, taskPoint, taskLayer);
        if (taskStatesPoints.Count > 0)
        {
            return new CharacterTask(taskStatesPoints, this);
        }
        else
        {
            return null;
        }
    }

    public CharacterActionState ActivateState(CharacterActionState state, Vector2 endPosition, Vector2 actionDirection)
    {
        CharacterAction actionData = new CharacterAction(this, skillsManager, state, transform.position, endPosition, actionDirection);

        CharacterActionHandler actionHandler = actionsHandlers.FirstOrDefault(handler => handler.HandledState == state);

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
                Gizmos.DrawSphere(point.CellPosition, 0.1f);
                Gizmos.DrawLine(point.CellPosition, point.NextCellToCharacterPosition);
            }
        }
    }
}
