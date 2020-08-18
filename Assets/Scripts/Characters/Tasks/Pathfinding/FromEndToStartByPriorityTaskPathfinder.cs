using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Поиск с конца (от целевой точки) до персонажа по приоритету клеток на основании A*
/// </summary>
[CreateAssetMenu(fileName = "FromEndToStartByPriorityTaskPathfinder", menuName = "TaskPathfinders/FromEndToStartByPriority")]

public class FromEndToStartByPriorityTaskPathfinder : TaskPathfinder
{
    [Header("По реквесту мы узнаем слой клетки")]
    [Tooltip("Реквесты используются в случае, когда префабам или скриптаблям нужно узнать что-то от объектов на сцене, замена прямого поиска")]
    [SerializeField] private CellLayoutRequest cellLayoutRequest;
    [Header("Реквест для определения нахождения персонажа")]
    [SerializeField] private Request checkIfCharacterOnCellRequest;
    [Header("Доступные состояния для клеток")]
    [SerializeField] private List<CharacterActionState> cellActionsStates = new List<CharacterActionState>();
    [Header("Максимальная глубина поиска")]
    [SerializeField] private int pathFindMaxDepth;

    private List<Vector2> checkedPositions = new List<Vector2>();

    // Доступные оси
    private Vector2Int[] axises = new Vector2Int[4] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };

    public override List<PathPoint> FindPath(Vector2 characterPosition, Vector2 taskPosition, int endPointLayer)
    {
        // Начинаем с конца, чтобы двигаться от меньшего приоритета (земли) к большему (тоннели)
        CharacterActionState taskEndPointState = cellActionsStates.Find(state => Utils.MaskToLayer(state.actionLayerMask) == endPointLayer);

        if (taskEndPointState != null)
        {
            // Создаем точку, в которую при поиске будут помещаться все остальные точки поиска. Первый слой - конечная точка
            PathPoint allAvailablePaths = new PathPoint(taskEndPointState, taskPosition, Vector2.zero);

            checkedPositions.Clear();
            // FindPathTreeToCharacter внутри обновляет allAvailablePaths, добавляя все возможные пути, а возвращает точки до персонажа
            List<PathPoint> lastPointsToCharacter = FindPointsToCharacter(allAvailablePaths.NestLevel, characterPosition, ref allAvailablePaths);

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

    /// <summary>
    /// Определить путь до цели по последним точкам (на основании вложенных точек)
    /// </summary>
    private List<PathPoint> GetPathFromLastPoints(List<PathPoint> lastPointsToCharacter)
    {
        List<PathPoint> pathToCharacter = new List<PathPoint>();
        lastPointsToCharacter[0].GetPointWithAllParents(ref pathToCharacter);
        Vector2 closerToCharacterPointPosition = pathToCharacter[0].PointPosition;
        // Первая точка в пути - это позиция персонажа, в ней не выполняется никаких действий, она нам больше не нужна
        pathToCharacter.RemoveAt(0);
        for (int i = 0; i < pathToCharacter.Count; i++)
        {
            pathToCharacter[i].closerToCharacterPointPosition = closerToCharacterPointPosition;
            closerToCharacterPointPosition = pathToCharacter[i].PointPosition;
        }
        return pathToCharacter;
    }

    public List<PathPoint> FindPointsToCharacter(int NestLevel, Vector2 characterPosition, ref PathPoint allAvailablePaths)
    {
        if (NestLevel <= pathFindMaxDepth)
        {
            // Получаем точки текущей итерации
            List<PathPoint> currentIterationPaths = allAvailablePaths.GetPointsWithNestLevel(NestLevel);
            // Проверяем клетки следующей итерации
            List<PathPoint> pointsToCharacter = CheckPathToCharacterAmongPaths(currentIterationPaths, characterPosition, ref allAvailablePaths);
            if (pointsToCharacter != null && pointsToCharacter.Count > 0)
                return pointsToCharacter;
            else
                return FindPointsToCharacter(NestLevel + 1, characterPosition, ref allAvailablePaths);
        }
        else
            return new List<PathPoint>() { };

    }

    ///<summary>Проверяет наличие персонажа на этой итерации и добавлять все проверки в allAvailablePaths</summary> 
    private List<PathPoint> CheckPathToCharacterAmongPaths(List<PathPoint> currentIterationPaths, Vector2 characterPosition, ref PathPoint allAvailablePaths)
    {
        // Словарь с путями, где ключ - это значение приоритета, значение - словарь, в котором ключ - это индекс точки в данной итерации,
        // а значение - доступные от него дальнейшие пути
        Dictionary<int, Dictionary<int, List<PathPoint>>> priorityPaths = new Dictionary<int, Dictionary<int, List<PathPoint>>>();
        for (int i = 0; i < currentIterationPaths.Count; i++)
        {
            PathPoint pathPoint = currentIterationPaths[i];
            Dictionary<int, List<PathPoint>> currentIterationPathsByPriority = new Dictionary<int, List<PathPoint>>();

            foreach (Vector2Int axis in axises)
            {
                if (axis != -pathPoint.axisFromParent)
                {
                    Vector2 actionPosition = pathPoint.PointPosition + axis;
                    if (!checkedPositions.Contains(actionPosition))
                    {
                        checkedPositions.Add(actionPosition);
                        // На клетке находится персонаж, значит мы нашли путь
                        if (checkIfCharacterOnCellRequest.MakeRequest(actionPosition, characterPosition))
                        {
                            List<PathPoint> pointsToCharacter = new List<PathPoint>() { new PathPoint(pathPoint.state, pathPoint.PointPosition, axis) };
                            allAvailablePaths.AddPointToPositionInPath(currentIterationPaths[i].positionInPath, pointsToCharacter);

                            return pointsToCharacter;
                        }
                        else if (cellLayoutRequest.MakeRequest(new ParamsObject(actionPosition), out LayerMask cellLayerMask))
                        {
                            CharacterActionState cellState = cellActionsStates.Find(state => Utils.MaskToLayer(state.actionLayerMask) == cellLayerMask);
                            if (cellState != null)
                            {
                                if (currentIterationPathsByPriority.ContainsKey(cellState.priority))
                                {
                                    currentIterationPathsByPriority[cellState.priority].Add(new PathPoint(cellState, pathPoint.PointPosition, axis));
                                }
                                else
                                {
                                    currentIterationPathsByPriority.Add(cellState.priority, new List<PathPoint>() { new PathPoint(cellState, pathPoint.PointPosition, axis) });
                                }
                            }
                        }
                    }                  
                }
            }

            if (currentIterationPathsByPriority.Count > 0)
            {
                List<PathPoint> morePrioritePaths = currentIterationPathsByPriority.Aggregate((biggest, next) => next.Key > biggest.Key ? next : biggest).Value;
                int currentIterationPriority = morePrioritePaths[0].Priority;
                // Добавляем в словарь с путями по приоритетам результаты проверки
                if (priorityPaths.ContainsKey(currentIterationPriority))
                {
                    priorityPaths[currentIterationPriority].Add(i, morePrioritePaths);
                }
                else
                {
                    priorityPaths.Add(currentIterationPriority, new Dictionary<int, List<PathPoint>>() { { i, morePrioritePaths } });
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
                    // Добавляем наиболее приоритетные точки в общее дерево путей
                    allAvailablePaths.AddPointToPositionInPath(currentIterationPaths[i].positionInPath, higherPriorityPaths[i]);
                }
            }
        }

        return null;
    }
}

