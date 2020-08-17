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
    private PathPoint finalPath;

    public override List<PathPoint> FindPath(Vector2 startPosition, Vector2 taskPoint, int taskLayer)
    {
        // Начинаем с конца, чтобы двигаться от меньшего приоритета (земли) к большему (тоннели)
        CharacterActionState taskEndPointState = cellActionsStates.Find(state => Utils.MaskToLayer(state.actionLayerMask) == taskLayer);

        if (taskEndPointState != null)
        {
            finalPath = new PathPoint(taskEndPointState, taskPoint, Vector2.zero);

            // FindPathTreeToCharacter внутри обновляет finalPath, добавляя все возможные пути, а возвращает eдинственный путь до персонажа
            List<PathPoint> lastPointsToCharacter = FindPathToCharacter(finalPath.nestLevel, startPosition, ref finalPath);

            if (lastPointsToCharacter != null && lastPointsToCharacter.Count > 0)
            {
                return GenerateGizmosPath(lastPointsToCharacter[0], startPosition);
            }
            else
            {
                Debugger.Log("Путь не найден", "red");
            }

        }
        return new List<PathPoint>();
    }

    public List<PathPoint> FindPathToCharacter(int nestLevel, Vector2 position, ref PathPoint finalPath)
    {
        if (nestLevel <= pathFindMaxDepth)
        {
            // Получаем точки текущей итерации
            List<PathPoint> currentIterationPaths = finalPath.GetAllStatesWithNestLevel(nestLevel);
            // Проверяем клетки следующей итерации
            List<PathPoint> pointsToCharacter = CheckPathToCharacterAmongPaths(currentIterationPaths, position, ref finalPath);

            return (pointsToCharacter != null && pointsToCharacter.Count > 0) ? pointsToCharacter : FindPathToCharacter(nestLevel + 1, position, ref finalPath);
        }
        else
            return new List<PathPoint>() { };

    }

    public List<PathPoint> GenerateGizmosPath(PathPoint lastPointsToCharacter, Vector2 startPosition)
    {
        List<PathPoint> pathToCharacter = new List<PathPoint>();
        lastPointsToCharacter.GetPointWithAllPrevs(ref pathToCharacter);
        // Для гизмо
        List<PathPoint> gizmosPath = new List<PathPoint>();
        Vector2 nextPosition = pathToCharacter[0].CellPosition;
        for (int i = 1; i < pathToCharacter.Count; i++)
        {
            pathToCharacter[i].NextCellToCharacterPosition = nextPosition;
            gizmosPath.Add(pathToCharacter[i]);
            nextPosition = pathToCharacter[i].CellPosition;
        }
        return gizmosPath;
    }

    // Должна проверять наличие персонажа на этой итерации и добавлять все проверки в finalPath
    public List<PathPoint> CheckPathToCharacterAmongPaths(List<PathPoint> currentIterationPaths, Vector2 position, ref PathPoint finalPath)
    {
        Dictionary<int, Dictionary<int, List<PathPoint>>> priorityPaths = new Dictionary<int, Dictionary<int, List<PathPoint>>>();
        for (int i = 0; i < currentIterationPaths.Count; i++)
        {
            (bool isCharacter, int pathsPriority, List<PathPoint> nextPaths) checkResult = CheckPathToCharacter(currentIterationPaths[i], position);

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
                    finalPath.AddPathsToCertainPositionInTree(currentIterationPaths[i].positionInPath, higherPriorityPaths[i]);
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
            if (axis != -prevPath.axisFromPrevCell)
            {
                Vector2 actionPosition = prevPath.CellPosition + axis;

                // На клетке находится персонаж, значит мы нашли путь
                if (checkIfCharacterOnCellRequest.MakeRequest(actionPosition, position))
                {
                    return (true, prevPath.Priority, new List<PathPoint>() { new PathPoint(prevPath.state, prevPath.CellPosition, axis) });
                }
                else if (cellLayoutRequest.MakeRequest(new ParamsObject(actionPosition), out LayerMask cellLayerMask))
                {
                    CharacterActionState cellState = cellActionsStates.Find(state => Utils.MaskToLayer(state.actionLayerMask) == cellLayerMask);
                    if (cellState != null)
                    {
                        if (pathsByPriority.ContainsKey(cellState.priority))
                        {
                            pathsByPriority[cellState.priority].Add(new PathPoint(cellState, prevPath.CellPosition, axis));
                        }
                        else
                        {
                            pathsByPriority.Add(cellState.priority, new List<PathPoint>() { new PathPoint(cellState, prevPath.CellPosition, axis) });
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

