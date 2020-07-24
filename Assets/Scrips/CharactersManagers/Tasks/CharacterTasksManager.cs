using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CharacterTask
{
    public Stack<(CharacterStateData state, Vector2 axisFromPrevCell)> stateActionsSteps = new Stack<(CharacterStateData, Vector2)>();

    public int currentStepIndex;

    public void AddStep(CharacterStateData state, Vector2 actionDirection)
    {
        stateActionsSteps.Push((state, actionDirection)); 
    }
}

public class CharacterTasksManager : MonoBehaviour
{
    [SerializeField] private CellLayoutRequest cellLayoutRequest;
    [SerializeField] private Request checkIfCharacterOnCellRequest;

    [SerializeField] private CharacterStateData idleState;
    [SerializeField] private List<CharacterStateData> cellActionsStates = new List<CharacterStateData>();
    [SerializeField] private CharacterTasksManagersSet set;
    [SerializeField] private CharacterInitialData initialData;

    [Header("Максимальная глубина поиска")]
    [SerializeField] private int pathFindMaxDepth;

    private Grid grid;

    Vector2Int[] axises = new Vector2Int[4] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };

    public void Start()
    {
        set.Add(this);

        grid = GetComponentInParent<Grid>();
    }

    public void OnDestroy()
    {
        set.Remove(this);
    }

    StateActionPoint finalPath;
    List<StateActionPoint> gizmosPath = new List<StateActionPoint>();

    public void ExecuteTask(GameObject taskObject, Vector2 taskPoint)
    {
        // Начинаем с конца, чтобы двигаться от меньшего приоритета (земли) к большему (тоннели)
        CharacterStateData taskEndPointState = cellActionsStates.Find(state => state.CompareActionMaskWithLayer(taskObject.layer));

        if (taskEndPointState != null)
        {
            finalPath = new StateActionPoint(taskEndPointState, taskPoint, Vector2.zero);

            // FindPathTreeToCharacter внутри обновляет finalPath, добавляя все возможно пути, а возвращает
            // единственный путь до персонажа
            List<StateActionPoint> lastPointsToCharacter = FindPathToCharacter(finalPath.nestLevel);            

            if (lastPointsToCharacter != null && lastPointsToCharacter.Count > 0)
            {
                GenerateGizmosPath(lastPointsToCharacter);
            }
            else
            {
                Debugger.Log("Путь не найден", "red");
            }

        }
    }

    private void GenerateGizmosPath(List<StateActionPoint> lastPointsToCharacter)
    {
        List<StateActionPoint> pathToCharacter = new List<StateActionPoint>();
        lastPointsToCharacter[0].GetPointWithAllPrevs(ref pathToCharacter);
        // Для гизмо
        gizmosPath.Clear();
        Vector2 nextPosition = transform.position;
        for (int i = 0; i < pathToCharacter.Count; i++)
        {
            pathToCharacter[i].NextCellToCharacterPosition = nextPosition;
            gizmosPath.Add(pathToCharacter[i]);
            nextPosition = pathToCharacter[i].CellPosition;
        }
    }

    private List<StateActionPoint> FindPathToCharacter(int nestLevel)
    {
        if (nestLevel <= pathFindMaxDepth )
        {
            // Получаем точки текущей итерации
            List<StateActionPoint> currentIterationPaths = finalPath.GetAllStatesWithNestLevel(nestLevel);
            
            // Проверяем клетки следующей итерации
            List<StateActionPoint> pointsToCharacter = CheckPathToCharacterAmongPaths(currentIterationPaths);

            return (pointsToCharacter != null && pointsToCharacter.Count > 0) ? pointsToCharacter : FindPathToCharacter(nestLevel + 1);
        }
        else
            return new List<StateActionPoint>() { };

    }

    // Должна проверять наличие персонажа на этой итерации и добавлять все проверки в finalPath
    private List<StateActionPoint> CheckPathToCharacterAmongPaths(List<StateActionPoint> currentIterationPaths)
    {
        Dictionary<int, Dictionary<int, List<StateActionPoint>>> priorityPaths = new Dictionary<int, Dictionary<int, List<StateActionPoint>>>();
        for (int i = 0; i < currentIterationPaths.Count; i++)
        {                   
            (bool isCharacter, int pathsPriority, List<StateActionPoint> nextPaths) checkResult = CheckPathToCharacter(currentIterationPaths[i]);

            if (checkResult.isCharacter)
            {
                finalPath.AddPathsToCertainPositionInTree(currentIterationPaths[i].positionInPath, checkResult.nextPaths);
                return checkResult.nextPaths;
            }
            else
            {
                if (priorityPaths.ContainsKey(checkResult.pathsPriority))
                {
                    priorityPaths[checkResult.pathsPriority].Add(i, checkResult.nextPaths);                  
                }
                else
                {
                    priorityPaths.Add(checkResult.pathsPriority, new Dictionary<int, List<StateActionPoint>>(){{i, checkResult.nextPaths }});
                }
            }
        }
        if (priorityPaths.Count > 0)
        {
            // Из priorityPaths забираем только один элемент с бОльшим приоритетом
            Dictionary<int, List<StateActionPoint>> higherPriorityPaths = priorityPaths.Aggregate((biggest, next) =>
            {
                return next.Key > biggest.Key ? next : biggest;
            }).Value;


            for (int i = 0; i < currentIterationPaths.Count; i++)
            {
                if (higherPriorityPaths.ContainsKey(i))
                {
                    finalPath.AddPathsToCertainPositionInTree(currentIterationPaths[i].positionInPath, higherPriorityPaths[i]);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns>bool - найден ли путь до персонажа, int - приоритет следующего состояния,  StateActionPoint - измененный path</returns>
    private (bool, int, List<StateActionPoint>) CheckPathToCharacter(StateActionPoint prevPath)
    {
        List<StateActionPoint> morePrioritePaths = new List<StateActionPoint>();
        List<StateActionPoint> samePrioritePaths = new List<StateActionPoint>();

        foreach (Vector2Int axis in axises)
        {
            if (axis != -prevPath.axisFromPrevCell)
            {
                Vector2 actionPosition = prevPath.CellPosition + axis;

                // На клетке находится персонаж, значит мы нашли путь
                if (checkIfCharacterOnCellRequest.MakeRequest(actionPosition, (Vector2)transform.position))
                {
                    return (true, prevPath.Priority, new List<StateActionPoint>() { new StateActionPoint(prevPath.state, prevPath.CellPosition, axis) });
                }
                else if (cellLayoutRequest.MakeRequest(new ParamsObject(actionPosition), out LayerMask cellLayerMask))
                {
                    CharacterStateData cellState = cellActionsStates.Find(state => state.CompareActionMaskWithLayer(cellLayerMask));
                    if (cellState != null)
                    {
                        if (cellState.actionPriority == prevPath.Priority)
                            samePrioritePaths.Add(new StateActionPoint(cellState, prevPath.CellPosition, axis));
                        else if (cellState.actionPriority > prevPath.Priority)
                            morePrioritePaths.Add(new StateActionPoint(cellState, prevPath.CellPosition, axis));
                    }
                }
            }
        }
        if (morePrioritePaths.Count > 0)
        {
            return (false, morePrioritePaths[0].Priority, morePrioritePaths);
        }
        else if (samePrioritePaths.Count > 0)
        {
            return (false, prevPath.Priority, samePrioritePaths);
        }
        return (false, 0, new List<StateActionPoint>());
    }


    public void OnBecomeNotActive() 
    {
    }
     
    public void OnBecomeActive() 
    {
    }

    public override string ToString()
    {
        return initialData.name;
    }

    bool getNewPath;

    public void OnDrawGizmos()
    {
        if (finalPath != null)
        {
            foreach(StateActionPoint point in gizmosPath)
            {
                Gizmos.color = point.state.gizmosColor;
                Gizmos.DrawSphere(point.CellPosition, 0.1f);
                Gizmos.DrawLine(point.CellPosition, point.NextCellToCharacterPosition);
            }
        }
    }
}
