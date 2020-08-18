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

    private PathPoint allAvailablePaths;
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
            allAvailablePaths = new PathPoint(taskEndPointState, taskPosition, Vector2.zero);

            checkedPositions.Clear();
            // FindPathTreeToCharacter внутри обновляет allAvailablePaths, добавляя все возможные пути, а возвращает точки до персонажа
            List<PathPoint> lastPointsToCharacter = FindPointsToCharacter(allAvailablePaths.NestLevel, characterPosition);

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

    public List<PathPoint> FindPointsToCharacter(int NestLevel, Vector2 characterPosition)
    {
        if (NestLevel <= pathFindMaxDepth)
        {
            // Получаем точки текущей итерации
            List<PathPoint> iterationPaths = allAvailablePaths.GetPointsWithNestLevel(NestLevel);

            // Проверяем клетки следующей итерации
            List<PathPoint> pointsToCharacter = CheckPathToCharacterAmongPaths(iterationPaths, characterPosition);

            if (pointsToCharacter != null && pointsToCharacter.Count > 0)
                return pointsToCharacter;
            else
                return FindPointsToCharacter(NestLevel + 1, characterPosition);
        }
        else
            return new List<PathPoint>() { };

    }

    ///<summary>Проверяет наличие персонажа на этой итерации и добавлять все проверки в allAvailablePaths</summary> 
    private List<PathPoint> CheckPathToCharacterAmongPaths(List<PathPoint> iterationPaths, Vector2 characterPosition)
    {
        // Словарь с путями, где ключ - это значение приоритета, значение - словарь, в котором ключ - это индекс точки в данной итерации,
        // а значение - доступные от него дальнейшие пути
        Dictionary<int, Dictionary<int, List<PathPoint>>> priorityPaths = new Dictionary<int, Dictionary<int, List<PathPoint>>>();
        for (int i = 0; i < iterationPaths.Count; i++)
        {
            PathPoint pathPoint = iterationPaths[i];
            Dictionary<int, List<PathPoint>> iterationPathsByPriority = new Dictionary<int, List<PathPoint>>();

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
                            allAvailablePaths.AddPathsToPositionInPath(iterationPaths[i].positionInPath, pointsToCharacter);

                            return pointsToCharacter;
                        }
                        else if (cellLayoutRequest.MakeRequest(new ParamsObject(actionPosition), out LayerMask cellLayerMask))
                        {
                            AddCellPoint(pathPoint.PointPosition, axis, cellLayerMask, ref iterationPathsByPriority);
                        }
                    }                  
                }
            }

            if (iterationPathsByPriority.Count > 0)
            {
                AddIterationHighestPriorityPaths(iterationPathsByPriority, i, ref priorityPaths);
            }
        }
        if (priorityPaths.Count > 0)
        {
            AddHighestPriorityPaths(iterationPaths, priorityPaths);
        }
        return null;
    }

    private void AddCellPoint(Vector2 pointPosition, Vector2 axis, LayerMask cellLayerMask, ref Dictionary<int, List<PathPoint>> iterationPathsByPriority)
    {

        CharacterActionState cellState = cellActionsStates.Find(state => Utils.MaskToLayer(state.actionLayerMask) == cellLayerMask);
        if (cellState != null)
        {
            if (iterationPathsByPriority.ContainsKey(cellState.priority))
            {
                iterationPathsByPriority[cellState.priority].Add(new PathPoint(cellState, pointPosition, axis));
            }
            else
            {
                iterationPathsByPriority.Add(cellState.priority, new List<PathPoint>() { new PathPoint(cellState, pointPosition, axis) });
            }
        }
    }

    private void AddIterationHighestPriorityPaths(Dictionary<int, List<PathPoint>> iterationPathsByPriority, int pathIndex, ref Dictionary<int, Dictionary<int, List<PathPoint>>> priorityPaths)
    {
        int currentIterationHighestPriority = 0;

        // Раньше здесь было Aggregate, который при большом количестве итераций выдавал ошибку в WebGL
        foreach (KeyValuePair<int, List<PathPoint>> morePrioritePathsPair in iterationPathsByPriority)
        {
            if (morePrioritePathsPair.Key > currentIterationHighestPriority)
            {
                currentIterationHighestPriority = morePrioritePathsPair.Key;
            }
        }
        List<PathPoint> morePrioritePaths = iterationPathsByPriority[currentIterationHighestPriority];

        // Добавляем в словарь с путями по приоритетам результаты проверки
        if (priorityPaths.ContainsKey(currentIterationHighestPriority))
        {
            priorityPaths[currentIterationHighestPriority].Add(pathIndex, morePrioritePaths);
        }
        else
        {
            priorityPaths.Add(currentIterationHighestPriority, new Dictionary<int, List<PathPoint>>() { { pathIndex, morePrioritePaths } });
        }
    }

    private void AddHighestPriorityPaths(List<PathPoint> iterationPaths, Dictionary<int, Dictionary<int, List<PathPoint>>> priorityPaths)
    {
        // Из priorityPaths забираем только один элемент с бОльшим приоритетом
        int highestPriorityAmongPaths = 0;

        // Раньше здесь было Aggregate, который при большом количестве итераций выдавал ошибку в WebGL
        foreach (KeyValuePair<int, Dictionary<int, List<PathPoint>>> priorityPathPair in priorityPaths)
        {
            if (priorityPathPair.Key > highestPriorityAmongPaths)
            {
                highestPriorityAmongPaths = priorityPathPair.Key;
            }
        }
        Dictionary<int, List<PathPoint>> higherPriorityPaths = priorityPaths[highestPriorityAmongPaths];

        for (int i = 0; i < iterationPaths.Count; i++)
        {
            if (higherPriorityPaths.ContainsKey(i))
            {
                // Добавляем наиболее приоритетные точки в общее дерево путей
                allAvailablePaths.AddPathsToPositionInPath(iterationPaths[i].positionInPath, higherPriorityPaths[i]);
            }
        }
    }
}

