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
    private CharacterTaskPoint finalPath;

    public override List<CharacterTaskPoint> FindPath(Vector2 startPosition, Vector2 taskPoint, int taskLayer)
    {
        // Начинаем с конца, чтобы двигаться от меньшего приоритета (земли) к большему (тоннели)
        CharacterStateData taskEndPointState = cellActionsStates.Find(state => state.CompareActionMaskWithLayer(taskLayer));

        if (taskEndPointState != null)
        {
            finalPath = new CharacterTaskPoint(taskEndPointState, taskPoint, Vector2.zero);

            // FindPathTreeToCharacter внутри обновляет finalPath, добавляя все возможно пути, а возвращает eдинственный путь до персонажа
            List<CharacterTaskPoint> lastPointsToCharacter = FindPathToCharacter(finalPath.nestLevel, startPosition, ref finalPath);

            if (lastPointsToCharacter != null && lastPointsToCharacter.Count > 0)
            {
                return GenerateGizmosPath(lastPointsToCharacter[0], startPosition);
            }
            else
            {
                Debugger.Log("Путь не найден", "red");
            }

        }
        return new List<CharacterTaskPoint>();
    }

    public List<CharacterTaskPoint> FindPathToCharacter(int nestLevel, Vector2 position, ref CharacterTaskPoint finalPath)
    {
        if (nestLevel <= pathFindMaxDepth)
        {
            // Получаем точки текущей итерации
            List<CharacterTaskPoint> currentIterationPaths = finalPath.GetAllStatesWithNestLevel(nestLevel);
            // Проверяем клетки следующей итерации
            List<CharacterTaskPoint> pointsToCharacter = CheckPathToCharacterAmongPaths(currentIterationPaths, position, ref finalPath);

            return (pointsToCharacter != null && pointsToCharacter.Count > 0) ? pointsToCharacter : FindPathToCharacter(nestLevel + 1, position, ref finalPath);
        }
        else
            return new List<CharacterTaskPoint>() { };

    }

    public List<CharacterTaskPoint> GenerateGizmosPath(CharacterTaskPoint lastPointsToCharacter, Vector2 startPosition)
    {
        List<CharacterTaskPoint> pathToCharacter = new List<CharacterTaskPoint>();
        lastPointsToCharacter.GetPointWithAllPrevs(ref pathToCharacter);
        // Для гизмо
        List<CharacterTaskPoint> gizmosPath = new List<CharacterTaskPoint>();
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
    public List<CharacterTaskPoint> CheckPathToCharacterAmongPaths(List<CharacterTaskPoint> currentIterationPaths, Vector2 position, ref CharacterTaskPoint finalPath)
    {
        Dictionary<int, Dictionary<int, List<CharacterTaskPoint>>> priorityPaths = new Dictionary<int, Dictionary<int, List<CharacterTaskPoint>>>();
        for (int i = 0; i < currentIterationPaths.Count; i++)
        {
            (bool isCharacter, int pathsPriority, List<CharacterTaskPoint> nextPaths) checkResult = CheckPathToCharacter(currentIterationPaths[i], position);

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
                    priorityPaths.Add(checkResult.pathsPriority, new Dictionary<int, List<CharacterTaskPoint>>() { { i, checkResult.nextPaths } });
                }
            }
        }
        if (priorityPaths.Count > 0)
        {
            // Из priorityPaths забираем только один элемент с бОльшим приоритетом
            Dictionary<int, List<CharacterTaskPoint>> higherPriorityPaths = priorityPaths.Aggregate((biggest, next) => next.Key > biggest.Key ? next : biggest).Value;


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

    public (bool, int, List<CharacterTaskPoint>) CheckPathToCharacter(CharacterTaskPoint prevPath, Vector2 position)
    {
        Dictionary<int, List<CharacterTaskPoint>> pathsByPriority = new Dictionary<int, List<CharacterTaskPoint>>();

        foreach (Vector2Int axis in axises)
        {
            if (axis != -prevPath.axisFromPrevCell)
            {
                Vector2 actionPosition = prevPath.CellPosition + axis;

                // На клетке находится персонаж, значит мы нашли путь
                if (checkIfCharacterOnCellRequest.MakeRequest(actionPosition, position))
                {
                    return (true, prevPath.Priority, new List<CharacterTaskPoint>() { new CharacterTaskPoint(prevPath.stateData, prevPath.CellPosition, axis) });
                }
                else if (cellLayoutRequest.MakeRequest(new ParamsObject(actionPosition), out LayerMask cellLayerMask))
                {
                    CharacterStateData cellState = cellActionsStates.Find(state => state.CompareActionMaskWithLayer(cellLayerMask));
                    if (cellState != null)
                    {
                        if (pathsByPriority.ContainsKey(cellState.actionPriority))
                        {
                            pathsByPriority[cellState.actionPriority].Add(new CharacterTaskPoint(cellState, prevPath.CellPosition, axis));
                        }
                        else
                        {
                            pathsByPriority.Add(cellState.actionPriority, new List<CharacterTaskPoint>() { new CharacterTaskPoint(cellState, prevPath.CellPosition, axis) });
                        }
                    }
                }
            }
        }
        List<CharacterTaskPoint> morePrioritePaths = pathsByPriority.Aggregate((biggest, next) => next.Key > biggest.Key ? next : biggest).Value;
        if (morePrioritePaths.Count > 0)
            return (false, morePrioritePaths[0].Priority, morePrioritePaths);
        else
            return (false, 0, new List<CharacterTaskPoint>());
    }
}

