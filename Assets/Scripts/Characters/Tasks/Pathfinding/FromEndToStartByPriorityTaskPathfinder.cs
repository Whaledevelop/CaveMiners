using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "FromEndToStartByPriorityTaskPathfinder", menuName = "TaskPathfinders/FromEndToStartByPriority")]

public class FromEndToStartByPriorityTaskPathfinder : TaskPathfinder
{
    [SerializeField] private CellLayoutRequest cellLayoutRequest;
    [SerializeField] private Request checkIfCharacterOnCellRequest;
    [SerializeField] private List<CharacterActionState> cellActionsStates = new List<CharacterActionState>();

    [Header("Максимальная глубина поиска")]
    [SerializeField] private int pathFindMaxDepth;

    private Vector2Int[] axises = new Vector2Int[4] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };

    public override List<PathPoint> FindPath(Vector2 startPosition, Vector2 taskPoint, int taskLayer)
    {
        // Начинаем с конца, чтобы двигаться от меньшего приоритета (земли) к большему (тоннели)
        CharacterActionState taskEndPointState = cellActionsStates.Find(state => Utils.MaskToLayer(state.actionLayerMask) == taskLayer);

        if (taskEndPointState != null)
        {
            PathPoint fullPathTree = new PathPoint(taskEndPointState, taskPoint, Vector2.zero);

            // FindPathTreeToCharacter внутри обновляет fullPathTree, добавляя все возможные пути, а возвращает eдинственный путь до персонажа
            List<PathPoint> lastPointsToCharacter = FindPathToCharacter(fullPathTree.NestLevel, startPosition, ref fullPathTree);

            if (lastPointsToCharacter != null && lastPointsToCharacter.Count > 0)
            {
                return GetPathFromLastPoints(lastPointsToCharacter);
            }
            else
            {
                Debugger.Log("Путь не найден", "red");
            }

        }
        return new List<PathPoint>();
    }

    private List<PathPoint> GetPathFromLastPoints(List<PathPoint> lastPointsToCharacter)
    {
        List<PathPoint> pathToCharacter = new List<PathPoint>();
        lastPointsToCharacter[0].GetPointWithAllParents(ref pathToCharacter);
        Vector2 nextToCharacterPointPosition = pathToCharacter[0].PointPosition;
        // Первая точка в пути - это позиция персонажа, в ней не выполняется никаких действий, она нам больше не нужна
        pathToCharacter.RemoveAt(0);
        for (int i = 0; i < pathToCharacter.Count; i++)
        {
            pathToCharacter[i].nextToCharacterPointPosition = nextToCharacterPointPosition;
            nextToCharacterPointPosition = pathToCharacter[i].PointPosition;
        }
        return pathToCharacter;
    }

    public List<PathPoint> FindPathToCharacter(int NestLevel, Vector2 position, ref PathPoint fullPathTree)
    {
        if (NestLevel <= pathFindMaxDepth)
        {
            // Получаем точки текущей итерации
            List<PathPoint> currentIterationPaths = fullPathTree.GetAllStatesWithNestLevel(NestLevel);
            // Проверяем клетки следующей итерации
            List<PathPoint> pointsToCharacter = CheckPathToCharacterAmongPaths(currentIterationPaths, position, ref fullPathTree);

            return (pointsToCharacter != null && pointsToCharacter.Count > 0) ? pointsToCharacter : FindPathToCharacter(NestLevel + 1, position, ref fullPathTree);
        }
        else
            return new List<PathPoint>() { };

    }

    // Должна проверять наличие персонажа на этой итерации и добавлять все проверки в fullPathTree
    public List<PathPoint> CheckPathToCharacterAmongPaths(List<PathPoint> currentIterationPaths, Vector2 position, ref PathPoint fullPathTree)
    {
        Dictionary<int, Dictionary<int, List<PathPoint>>> priorityPaths = new Dictionary<int, Dictionary<int, List<PathPoint>>>();
        for (int i = 0; i < currentIterationPaths.Count; i++)
        {
            (bool isCharacter, int pathsPriority, List<PathPoint> nextPaths) checkResult = CheckPathToCharacter(currentIterationPaths[i], position);

            if (checkResult.isCharacter)
            {
                fullPathTree.AddPathsToCertainPositionInTree(currentIterationPaths[i].positionInPath, checkResult.nextPaths);
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
                    priorityPaths.Add(checkResult.pathsPriority, new Dictionary<int, List<PathPoint>>() { { i, checkResult.nextPaths } });
                }
            }
        }
        if (priorityPaths.Count > 0)
        {
            // Из priorityPaths забираем только один элемент с бОльшим приоритетом
            Dictionary<int, List<PathPoint>> higherPriorityPaths = priorityPaths.Aggregate((biggest, next) => next.Key > biggest.Key ? next : biggest).Value;


            for (int i = 0; i < currentIterationPaths.Count; i++)
            {
                if (higherPriorityPaths.ContainsKey(i))
                {
                    fullPathTree.AddPathsToCertainPositionInTree(currentIterationPaths[i].positionInPath, higherPriorityPaths[i]);
                }
            }
        }

        return null;
    }

    public (bool, int, List<PathPoint>) CheckPathToCharacter(PathPoint prevPath, Vector2 position)
    {
        Dictionary<int, List<PathPoint>> pathsByPriority = new Dictionary<int, List<PathPoint>>();

        foreach (Vector2Int axis in axises)
        {
            if (axis != -prevPath.axisFromParent)
            {
                Vector2 actionPosition = prevPath.PointPosition + axis;

                // На клетке находится персонаж, значит мы нашли путь
                if (checkIfCharacterOnCellRequest.MakeRequest(actionPosition, position))
                {
                    return (true, prevPath.Priority, new List<PathPoint>() { new PathPoint(prevPath.state, prevPath.PointPosition, axis) });
                }
                else if (cellLayoutRequest.MakeRequest(new ParamsObject(actionPosition), out LayerMask cellLayerMask))
                {
                    CharacterActionState cellState = cellActionsStates.Find(state => Utils.MaskToLayer(state.actionLayerMask) == cellLayerMask);
                    if (cellState != null)
                    {
                        if (pathsByPriority.ContainsKey(cellState.priority))
                        {
                            pathsByPriority[cellState.priority].Add(new PathPoint(cellState, prevPath.PointPosition, axis));
                        }
                        else
                        {
                            pathsByPriority.Add(cellState.priority, new List<PathPoint>() { new PathPoint(cellState, prevPath.PointPosition, axis) });
                        }
                    }
                }
            }
        }

        if (pathsByPriority.Count > 0)
        {
            List<PathPoint> morePrioritePaths = pathsByPriority.Aggregate((biggest, next) => next.Key > biggest.Key ? next : biggest).Value;
            if (morePrioritePaths.Count > 0)
                return (false, morePrioritePaths[0].Priority, morePrioritePaths);                
        }       
        return (false, 0, new List<PathPoint>());

    }
}

