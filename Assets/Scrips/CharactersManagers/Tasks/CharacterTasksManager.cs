using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterTasksManager : MonoBehaviour
{
    [SerializeField] private CharacterTasksManagersSet set;
    [SerializeField] private CharacterInitialData initialData;
    [SerializeField] private TaskPathfinder taskPathfinder;

    private List<StateActionPoint> activeTask = new List<StateActionPoint>();

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
        activeTask = taskPathfinder.FindPath(transform.position, taskObject, taskPoint);
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
        foreach (StateActionPoint point in activeTask)
        {
            Gizmos.color = point.state.gizmosColor;
            Gizmos.DrawSphere(point.CellPosition, 0.1f);
            Gizmos.DrawLine(point.CellPosition, point.NextCellToCharacterPosition);
        }
    }
}
