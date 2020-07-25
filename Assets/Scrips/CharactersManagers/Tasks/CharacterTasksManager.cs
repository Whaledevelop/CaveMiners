using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTasksManager : MonoBehaviour
{
    [SerializeField] private CharacterTasksManagersSet set;
    [SerializeField] private CharacterInitialData initialData;
    [SerializeField] private CharacterStatesManager statesManager;
    [SerializeField] private TaskPathfinder taskPathfinder;
    [SerializeField] private Highlighter characterHighlighter;

    private CharacterTask activeTask;

    public void Start()
    {
        set.Add(this);
    }

    public void OnDestroy()
    {
        set.Remove(this);
    }    
    
    public void ExecuteTask(GameObject taskObject, Vector2 taskPoint)
    {
        if (activeTask != null)
            activeTask.Cancel();
        List<CharacterTaskPoint> taskStatesPoints = taskPathfinder.FindPath(transform.position, taskObject, taskPoint);
        activeTask = new CharacterTask(taskStatesPoints, statesManager);
        activeTask.Start();
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
