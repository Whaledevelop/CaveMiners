using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTask
{
    public List<StateAction> statesPoints = new List<StateAction>();
    public int currentStateIndex = -1;
    public bool isActive;

    public CharacterStatesManager statesManager;

    public Action OnEnd;

    public CharacterTask(List<StateAction> statesPoints, CharacterStatesManager statesManager)
    {
        this.statesPoints = statesPoints;
        this.statesManager = statesManager;
    }


    public void Start()
    {
        SetNextState();
        statesManager.onEndState += SetNextState;
    }

    public void SetNextState()
    {
        currentStateIndex++;
        if (currentStateIndex < statesPoints.Count)
        {
            statesManager.SetState(statesPoints[currentStateIndex].state, statesPoints[currentStateIndex].CellPosition);
        }
        else
        {
            End();
        }
              
    }

    public void Cancel() { }

    public void End()
    {
        statesManager.onEndState -= SetNextState;
        OnEnd?.Invoke();
    }
}

public class CharacterTasksManager : MonoBehaviour
{
    [SerializeField] private CharacterTasksManagersSet set;
    [SerializeField] private CharacterInitialData initialData;
    [SerializeField] private CharacterStatesManager statesManager;
    [SerializeField] private TaskPathfinder taskPathfinder;

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
        List<StateAction> taskStatesPoints = taskPathfinder.FindPath(transform.position, taskObject, taskPoint);
        activeTask = new CharacterTask(taskStatesPoints, statesManager);
        activeTask.Start();
        activeTask.OnEnd += () =>
        {
            Debug.Log("Конец задачи");
        };
    }

    public void OnBecomeNotActive() 
    {
    }
     
    public void OnBecomeActive() 
    {
    }

    public override string ToString() => initialData.name;

    public void OnDrawGizmos()
    {
        if (activeTask != null)
        {
            foreach (StateAction point in activeTask.statesPoints)
            {
                Gizmos.color = point.state.gizmosColor;
                Gizmos.DrawSphere(point.CellPosition, 0.1f);
                Gizmos.DrawLine(point.CellPosition, point.NextCellToCharacterPosition);
            }
        }

    }
}
