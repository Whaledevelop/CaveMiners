using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TaskStartData
{
    public int taskLayer;
    public Vector2 taskPosition;
    public NoParamsDelegate executeDelegate;
    public TaskStartData(int taskLayer, Vector2 taskPosition, NoParamsDelegate executeDelegate = null)
    {
        this.taskLayer = taskLayer;
        this.taskPosition = taskPosition;
        this.executeDelegate = executeDelegate;
    }
}
public delegate void NoParamsDelegate();
public class CharacterTasksManager : MonoBehaviour
{

    [SerializeField] private CellPositionRequest cellCenterRequest;
    [SerializeField] private CharacterTasksManagersSet set;
    [SerializeField] private CharacterInitialData initialData;
    [SerializeField] private TaskPathfinder taskPathfinder;
    [SerializeField] private Highlighter characterHighlighter;
    [SerializeField] private CharacterStateData idleState;

    [SerializeField] private CharacterToolsManager toolsManager;
    [SerializeField] private CharacterSkillsManager skillsManager;
    [SerializeField] private Rotator rotator;
    [SerializeField] private Animator animator;

    [HideInInspector] public List<TaskStartData> nextTasks = new List<TaskStartData>();
    [HideInInspector] public List<TaskStartData> tasksHistory = new List<TaskStartData>();
    private CharacterTask activeTask;

    
    public Action onEndTask;

    public void Start()
    {
        set.Add(this);
    }

    public void OnDestroy()
    {
        set.Remove(this);
    }

    public void ExecuteTask(TaskStartData taskStartData)
    {
        ExecuteTask(taskStartData.taskLayer, taskStartData.taskPosition, taskStartData.executeDelegate);
    }

    public void ExecuteTask(int taskLayer, Vector2 taskPoint, NoParamsDelegate executeDelegate = null)
    {
        //    if (activeTask != null)
        //        StartCoroutine(activeTask.Cancel());
        cellCenterRequest.MakeRequest(new ParamsObject(taskPoint), out taskPoint);
        List<CharacterTaskPoint> taskStatesPoints = taskPathfinder.FindPath(transform.position, taskPoint, taskLayer);
        activeTask = new CharacterTask(taskStatesPoints, this, toolsManager, skillsManager, animator, rotator);        
        StartCoroutine(activeTask.ExecuteNextState());
        tasksHistory.Add(new TaskStartData(taskLayer, taskPoint));
        if (executeDelegate != null)
            onEndTask += executeDelegate.Invoke;
    }

    public void OnEndTask()
    {
        onEndTask?.Invoke();
        onEndTask = null;
        if (nextTasks.Count > 0)
        {
            ExecuteTask(nextTasks[0]);
            nextTasks.RemoveAt(0);
        }        
    }

    public void RepeatTaskFromHistory(int prevTaskIndexFromCurrent)
    {
        if (tasksHistory.Count > 0 && tasksHistory.Count - prevTaskIndexFromCurrent > 0)
        {
            TaskStartData taskFromHistoryStartData = tasksHistory[tasksHistory.Count - prevTaskIndexFromCurrent];
            ExecuteTask(taskFromHistoryStartData);
        }        
    }

    public void OnBecomeNotActive() 
    {
        characterHighlighter.SwapHighlightMode();
    }
     
    public void OnBecomeActive() 
    {
        characterHighlighter.SwapHighlightMode();
    }

    public override string ToString() => initialData.name;

    public void OnDrawGizmos()
    {
        if (activeTask != null)
        {
            foreach (CharacterTaskPoint point in activeTask.taskPoints)
            {
                Gizmos.color = point.stateData.gizmosColor;
                Gizmos.DrawSphere(point.CellPosition, 0.1f);
                Gizmos.DrawLine(point.CellPosition, point.NextCellToCharacterPosition);
            }
        }

    }
}
