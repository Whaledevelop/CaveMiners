using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "FromEndToStartByPriorityTaskPathfinder", menuName = "TaskPathfinders/FromEndToStartByPriority")]

public class FromEndToStartByPriorityTaskPathfinder : TaskPathfinder
{
    [SerializeField] private CellLayoutRequest cellLayoutRequest;
    [SerializeField] private Request checkIfCharacterOnCellRequest;
    [SerializeField] private List<CharacterStateData> cellActionsStates = new List<CharacterStateData>();
    [Header("Максимальная глубина поиска")]
    [SerializeField] private int pathFindMaxDepth;

    private Vector2Int[] axises = new Vector2Int[4] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
    private StateActionPoint finalPath;

    public override List<StateActionPoint> FindPath(Vector2 startPosition, GameObject taskObject, Vector2 taskPoint)
    {
        // Начинаем с конца, чтобы двигаться от меньшего приоритета (земли) к большему (тоннели)
        CharacterStateData taskEndPointState = cellActionsStates.Find(state => state.CompareActionMaskWithLayer(taskObject.layer));

        if (taskEndPointState != null)
        {
            finalPath = new StateActionPoint(taskEndPointState, taskPoint, Vector2.zero);

            // FindPathTreeToCharacter внутри обновляет finalPath, добавляя все возможно пути, а возвращает
            // единственный путь до персонажа
            List<StateActionPoint> lastPointsToCharacter = FindPathToCharacter(finalPath.nestLevel, startPosition, ref finalPath);

            if (lastPointsToCharacter != null && lastPointsToCharacter.Count > 0)
            {
                return GenerateGizmosPath(lastPointsToCharacter[0], startPosition);
            }
            else
            {
                Debugger.Log("Путь не найден", "red");
            }

        }
        return new List<StateActionPoint>();
    }

    public List<StateActionPoint> FindPathToCharacter(int nestLevel, Vector2 position, ref StateActionPoint finalPath)
    {
        if (nestLevel <= pathFindMaxDepth)
        {
            // Получаем точки текущей итерации
            List<StateActionPoint> currentIterationPaths = finalPath.GetAllStatesWithNestLevel(nestLevel);

            // Проверяем клетки следующей итерации
            List<StateActionPoint> pointsToCharacter = CheckPathToCharacterAmongPaths(currentIterationPaths, position, ref finalPath);

            return (pointsToCharacter != null && pointsToCharacter.Count > 0) ? pointsToCharacter : FindPathToCharacter(nestLevel + 1, position, ref finalPath);
        }
        else
            return new List<StateActionPoint>() { };

    }

    public List<StateActionPoint> GenerateGizmosPath(StateActionPoint lastPointsToCharacter, Vector2 startPosition)
    {
        List<StateActionPoint> pathToCharacter = new List<StateActionPoint>();
        lastPointsToCharacter.GetPointWithAllPrevs(ref pathToCharacter);
        // Для гизмо
        List<StateActionPoint> gizmosPath = new List<StateActionPoint>();
        Vector2 nextPosition = startPosition;
        for (int i = 0; i < pathToCharacter.Count; i++)
        {
            pathToCharacter[i].NextCellToCharacterPosition = nextPosition;
            gizmosPath.Add(pathToCharacter[i]);
            nextPosition = pathToCharacter[i].CellPosition;
        }
        return gizmosPath;
    }

    // Должна проверять наличие персонажа на этой итерации и добавлять все проверки в finalPath
    public List<StateActionPoint> CheckPathToCharacterAmongPaths(List<StateActionPoint> currentIterationPaths, Vector2 position, ref StateActionPoint finalPath)
    {
        Dictionary<int, Dictionary<int, List<StateActionPoint>>> priorityPaths = new Dictionary<int, Dictionary<int, List<StateActionPoint>>>();
        for (int i = 0; i < currentIterationPaths.Count; i++)
        {
            (bool isCharacter, int pathsPriority, List<StateActionPoint> nextPaths) checkResult = CheckPathToCharacter(currentIterationPaths[i], position);

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
                    priorityPaths.Add(checkResult.pathsPriority, new Dictionary<int, List<StateActionPoint>>() { { i, checkResult.nextPaths } });
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

    public (bool, int, List<StateActionPoint>) CheckPathToCharacter(StateActionPoint prevPath, Vector2 position)
    {
        List<StateActionPoint> morePrioritePaths = new List<StateActionPoint>();
        List<StateActionPoint> samePrioritePaths = new List<StateActionPoint>();

        foreach (Vector2Int axis in axises)
        {
            if (axis != -prevPath.axisFromPrevCell)
            {
                Vector2 actionPosition = prevPath.CellPosition + axis;

                // На клетке находится персонаж, значит мы нашли путь
                if (checkIfCharacterOnCellRequest.MakeRequest(actionPosition, position))
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
}

