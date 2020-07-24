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
    private StateAction finalPath;

    public override List<StateAction> FindPath(Vector2 startPosition, GameObject taskObject, Vector2 taskPoint)
    {
        // Начинаем с конца, чтобы двигаться от меньшего приоритета (земли) к большему (тоннели)
        CharacterStateData taskEndPointState = cellActionsStates.Find(state => state.CompareActionMaskWithLayer(taskObject.layer));

        if (taskEndPointState != null)
        {
            finalPath = new StateAction(taskEndPointState, taskPoint, Vector2.zero);

            // FindPathTreeToCharacter внутри обновляет finalPath, добавляя все возможно пути, а возвращает
            // единственный путь до персонажа
            List<StateAction> lastPointsToCharacter = FindPathToCharacter(finalPath.nestLevel, startPosition, ref finalPath);

            if (lastPointsToCharacter != null && lastPointsToCharacter.Count > 0)
            {
                return GenerateGizmosPath(lastPointsToCharacter[0], startPosition);
            }
            else
            {
                Debugger.Log("Путь не найден", "red");
            }

        }
        return new List<StateAction>();
    }

    public List<StateAction> FindPathToCharacter(int nestLevel, Vector2 position, ref StateAction finalPath)
    {
        if (nestLevel <= pathFindMaxDepth)
        {
            // Получаем точки текущей итерации
            List<StateAction> currentIterationPaths = finalPath.GetAllStatesWithNestLevel(nestLevel);

            // Проверяем клетки следующей итерации
            List<StateAction> pointsToCharacter = CheckPathToCharacterAmongPaths(currentIterationPaths, position, ref finalPath);

            return (pointsToCharacter != null && pointsToCharacter.Count > 0) ? pointsToCharacter : FindPathToCharacter(nestLevel + 1, position, ref finalPath);
        }
        else
            return new List<StateAction>() { };

    }

    public List<StateAction> GenerateGizmosPath(StateAction lastPointsToCharacter, Vector2 startPosition)
    {
        List<StateAction> pathToCharacter = new List<StateAction>();
        lastPointsToCharacter.GetPointWithAllPrevs(ref pathToCharacter);
        // Для гизмо
        List<StateAction> gizmosPath = new List<StateAction>();
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
    public List<StateAction> CheckPathToCharacterAmongPaths(List<StateAction> currentIterationPaths, Vector2 position, ref StateAction finalPath)
    {
        Dictionary<int, Dictionary<int, List<StateAction>>> priorityPaths = new Dictionary<int, Dictionary<int, List<StateAction>>>();
        for (int i = 0; i < currentIterationPaths.Count; i++)
        {
            (bool isCharacter, int pathsPriority, List<StateAction> nextPaths) checkResult = CheckPathToCharacter(currentIterationPaths[i], position);

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
                    priorityPaths.Add(checkResult.pathsPriority, new Dictionary<int, List<StateAction>>() { { i, checkResult.nextPaths } });
                }
            }
        }
        if (priorityPaths.Count > 0)
        {
            // Из priorityPaths забираем только один элемент с бОльшим приоритетом
            Dictionary<int, List<StateAction>> higherPriorityPaths = priorityPaths.Aggregate((biggest, next) =>
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

    public (bool, int, List<StateAction>) CheckPathToCharacter(StateAction prevPath, Vector2 position)
    {
        List<StateAction> morePrioritePaths = new List<StateAction>();
        List<StateAction> samePrioritePaths = new List<StateAction>();

        foreach (Vector2Int axis in axises)
        {
            if (axis != -prevPath.axisFromPrevCell)
            {
                Vector2 actionPosition = prevPath.CellPosition + axis;

                // На клетке находится персонаж, значит мы нашли путь
                if (checkIfCharacterOnCellRequest.MakeRequest(actionPosition, position))
                {
                    return (true, prevPath.Priority, new List<StateAction>() { new StateAction(prevPath.state, prevPath.CellPosition, axis) });
                }
                else if (cellLayoutRequest.MakeRequest(new ParamsObject(actionPosition), out LayerMask cellLayerMask))
                {
                    CharacterStateData cellState = cellActionsStates.Find(state => state.CompareActionMaskWithLayer(cellLayerMask));
                    if (cellState != null)
                    {
                        if (cellState.actionPriority == prevPath.Priority)
                            samePrioritePaths.Add(new StateAction(cellState, prevPath.CellPosition, axis));
                        else if (cellState.actionPriority > prevPath.Priority)
                            morePrioritePaths.Add(new StateAction(cellState, prevPath.CellPosition, axis));
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
        return (false, 0, new List<StateAction>());
    }
}

