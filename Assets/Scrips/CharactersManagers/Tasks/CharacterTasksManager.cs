using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTask
{
    public Stack<(CharacterStateData state, Vector2 actionAxis)> stateActionsSteps = new Stack<(CharacterStateData, Vector2)>();

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

    StateActionInPath finalPath;


    public void ExecuteTask(GameObject taskObject, Vector2 taskPoint)
    {
        // Начинаем с конца, чтобы двигаться от меньшего приоритета (земли) к большему (тоннели)
        CharacterStateData taskEndPointState = cellActionsStates.Find(state => state.CompareActionMaskWithLayer(taskObject.layer));

        if (taskEndPointState != null)
        {
            getNewPath = true;
            lengthToPlayer = -1;

            finalPath = new StateActionInPath(taskEndPointState, taskPoint, Vector2.zero);
            StartCoroutine(FindPath(finalPath, new List<int> { 0 }));
        }

    }

    private int lengthToPlayer = -1;

    private int checkNestLevel;

    private StateActionInPath FindPath(StateActionInPath path)
    {
        if (CheckAvailablePaths(ref path))
            return path;
        else
        {
            for(int i = 0; i < path.availablePaths.Count; i++)
            {
                path = FindPath(path.availablePaths[i]);
            }
        }
        return path;
    }

    private bool CheckAvailablePaths(ref StateActionInPath path)
    {
        bool isCharacter = FindCellPaths(path, out List<StateActionInPath> availablePaths);
        path.AddPathsFromCell(availablePaths);
        return isCharacter;
        //{
            
        //    return path;
        //}
        //else
        //{
        //    path.AddPathsFromCell(availablePaths);

        //    //List<StateActionInPath> nextNestLevelPaths = new List<StateActionInPath>();
        //    //path.GetAllStatesWithNestLevel(ref nextNestLevelPaths, path.nestLevel + 1);
        //}
    }

    #region Первоначальный вариант FindPath
    //private StateActionInPath FindPath(StateActionInPath path)
    //{
    //    //Debugger.Log(path.nestLevel + ", " + lengthToPlayer);
    //    //if (lengthToPlayer == -1 || (lengthToPlayer != -1 && path.nestLevel <= lengthToPlayer))
    //    //{
    //    //}

    //    if (FindCellHigherPrioritivePaths(path, out List<StateActionInPath> availablePaths))
    //    {
    //        path.AddPathsFromCell(availablePaths);
    //        lengthToPlayer = path.nestLevel;
    //        Debugger.Log(lengthToPlayer, "red");
    //    }
    //    else
    //    {
    //        path.AddPathsFromCell(availablePaths);
    //        // TODO : Сейчас идет проверка ветки за ветки, а надо проверять слой за слоем. Т.е. провести итерацию,
    //        // и только потом проверять новый слой, а не сначала все слои одной ветки, потом все слои другой и т.д.
    //        for (int i = 0; i < path.availablePaths.Count; i++)
    //        {

    //            StateActionInPath innerPath = FindPath(path.availablePaths[i]);
    //            path.availablePaths[i].AddPathFromCell(innerPath);
    //        }
    //    }

    //    return path;
    //}
    #endregion

    #region Варинт с карутиной
    private IEnumerator FindPath(StateActionInPath path, List<int> coordinatesInPathTree)
    {
        //Debugger.Log(path.nestLevel + ", " + lengthToPlayer);
        //if (lengthToPlayer == -1 || (lengthToPlayer != -1 && path.nestLevel <= lengthToPlayer))
        //{
        //}
        yield return new WaitUntil(() => path.nestLevel <= coordinatesInPathTree.Count);
        if (FindCellHigherPrioritivePaths(path, out List<StateActionInPath> availablePaths))
        {
            path.AddPathsFromCell(availablePaths);
            lengthToPlayer = path.nestLevel;
            Debugger.Log(lengthToPlayer, "red");
        }
        else
        {
            path.AddPathsFromCell(availablePaths);
            // TODO : Сейчас идет проверка ветки за ветки, а надо проверять слой за слоем. Т.е. провести итерацию,
            // и только потом проверять новый слой, а не сначала все слои одной ветки, потом все слои другой и т.д.
            for (int i = 0; i < path.availablePaths.Count; i++)
            {
                //StartCoroutine(FindPath(path.availablePaths[i]));
                //path.availablePaths[i].AddPathFromCell(innerPath);
            }
        }
    }
    #endregion
    private bool FindCellPaths(StateActionInPath prevPath, out List<StateActionInPath> nextPaths)
    {
        nextPaths = new List<StateActionInPath>();
        List<StateActionInPath> morePrioriteStates = new List<StateActionInPath>();
        List<StateActionInPath> samePrioriteStates = new List<StateActionInPath>();

        foreach (Vector2Int axis in axises)
        {
            if (axis != -prevPath.actionAxis)
            {
                Vector2 actionPosition = prevPath.ToCellPosition + axis;

                // На клетке находится персонаж, значит мы нашли путь
                if (checkIfCharacterOnCellRequest.MakeRequest(actionPosition, (Vector2)transform.position))
                {
                    nextPaths = new List<StateActionInPath>() { new StateActionInPath(prevPath.state, prevPath.ToCellPosition, axis) };
                    // Проверять другие оси нет смысла, т.к. на них уже не может быть данного персонажа
                    return true;
                }
                else if (cellLayoutRequest.MakeRequest(new ParamsObject(actionPosition), out LayerMask cellLayerMask))
                {
                    CharacterStateData cellState = cellActionsStates.Find(state => state.CompareActionMaskWithLayer(cellLayerMask));
                    if (cellState != null)
                    {
                        if (cellState.actionPriority == prevPath.Priority)
                            samePrioriteStates.Add(new StateActionInPath(cellState, prevPath.fromCellPosition, axis));
                        else if (cellState.actionPriority > prevPath.Priority)
                            morePrioriteStates.Add(new StateActionInPath(cellState, prevPath.fromCellPosition, axis));
                    }
                }
            }
        }
        nextPaths = morePrioriteStates.Count == 0 ? samePrioriteStates : morePrioriteStates;
        return false;
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
    List<List<Color>> colors;
    List<List<Vector2>> vectorPaths;

    public void OnDrawGizmos()
    {
        //if (finalPath != null)
        //{
        //    if (getNewPath)
        //    {
        //        vectorPaths = new List<List<Vector2>>();
        //        colors = new List<List<Color>>();

        //        for (int i = 0; i < finalPath.availablePaths.Count; i++)
        //        {
        //            List<Vector2> innerPaths = finalPath.availablePaths[i].GetVector2Points(new List<Vector2>() { finalPath.ToCellPosition });
        //            List<Color> innerPathsColors = new List<Color>();
        //            foreach (Vector2 vector2 in innerPaths)
        //            {
        //                innerPathsColors.Add(Utils.RandomColor());
        //            }
        //            colors.Add(innerPathsColors);
        //            vectorPaths.Add(innerPaths);
        //        }
        //        getNewPath = false;
        //    }
        //    for (int i = 0; i < vectorPaths.Count; i++)
        //    {
        //        for (int j = 0; j < vectorPaths[i].Count; j++)
        //        {
        //            Gizmos.color = colors[i][j];
        //            Gizmos.DrawSphere(vectorPaths[i][j], 0.05f);
        //        }
        //    }
        //}
    }
}
