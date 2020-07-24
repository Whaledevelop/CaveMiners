using System.Collections.Generic;
using UnityEngine;
public class StateActionPoint
{
    public CharacterStateData state;
    public Vector2 prevCellPosition;
    public Vector2 axisFromPrevCell;

    public StateActionPoint prevPoint;
    public int nestLevel = 1;


    public List<int> positionInPath = new List<int>();
    public List<StateActionPoint> nextActionsPoints = new List<StateActionPoint>();
    public string Name => state.stateName;
    public int Priority => state.actionPriority;
    public Vector2 CellPosition => prevCellPosition + axisFromPrevCell;

    

    #region Для гизмо

    public Vector2 axisToCharacter;
    public Vector2 NextCellToCharacterPosition;// => CellPosition + axisToCharacter;

    #endregion

    public StateActionPoint(CharacterStateData state, Vector2 prevCellPosition, Vector2 axisFromPrevCell)
    {
        this.state = state;
        this.prevCellPosition = prevCellPosition;
        this.axisFromPrevCell = axisFromPrevCell;
    }   

    public void GetPointWithAllPrevs(ref List<StateActionPoint> nextPoints)
    {
        nextPoints.Add(this);
        //Debugger.Log(prevPoint);
        if (prevPoint != null)
            prevPoint.GetPointWithAllPrevs(ref nextPoints);
    }

    public void AddPathsToCertainPositionInTree(List<int> parentPositionInPath, List<StateActionPoint> paths)
    {
        //("AddPathsToCertainPositionInTree - start", nestLevel, indexInNestLevel, paths.Count);
        //Debugger.LogIEnumerable(positionInPath);

        if (positionInPath == parentPositionInPath) // Если это батя
        {
            for (int i = 0; i < paths.Count; i++)
            {
                paths[i].positionInPath = new List<int>(positionInPath) { i };
                paths[i].nestLevel = nestLevel + 1;
                paths[i].prevPoint = this;
            }
            nextActionsPoints.AddRange(paths);
        }
        else if (positionInPath.Count < parentPositionInPath.Count) // Проверяем предка
        {
            for(int i = 0; i < positionInPath.Count; i++)
            {
                if (positionInPath[i] == parentPositionInPath[i])
                {
                    continue;
                }
                else
                    return; // Не так ветка
            }
            foreach (StateActionPoint availablePath in nextActionsPoints)
            {
                availablePath.AddPathsToCertainPositionInTree(parentPositionInPath, paths);
            }
        }
        //PrivateDebugger("AddPathsToCertainPositionInTree - end", nextActionsPoints.Count);
    }

    private void PrivateDebugger(string methodName, params object[] logStrings)
    {
        //Debugger.LogMethod(methodName + "(" + NestLevel, logStrings);
    }

    //public StateActionPoint GetPointByPositionInTree(int nestLevel, int index)
    //{
    //    PrivateDebugger("GetPointByPositionInTree", nestLevel, index, this);
    //    if (NestLevel == nestLevel)
    //    {
    //        return this;
    //    }
    //    else if (NestLevel == nestLevel - 1) // Если это батя
    //    {
    //        return nextActionsPoints[index];
    //    }
    //    else if (NestLevel < nestLevel - 1) // Если это предок
    //    {
    //        foreach (StateActionPoint availablePath in nextActionsPoints)
    //        {
    //            StateActionPoint innerPath = availablePath.GetPointByPositionInTree(nestLevel, index);
    //            if (innerPath != null)
    //                return innerPath;
    //        }
    //    }
    //    return null;
    //}

    public List<StateActionPoint> GetAllStatesWithNestLevel(int nestLevel)
    {
        List<StateActionPoint> states = new List<StateActionPoint>();

        PrivateDebugger("GetAllStatesWithNestLevel - start", nestLevel, states.Count);
        if (this.nestLevel == nestLevel)
        {
            states.Add(this);
        }
        else if (this.nestLevel < nestLevel)
        {
            for (int i = 0; i < nextActionsPoints.Count; i++)
            {
                states.AddRange(nextActionsPoints[i].GetAllStatesWithNestLevel(nestLevel));
            }
        }
        PrivateDebugger("GetAllStatesWithNestLevel - end", nestLevel, states.Count);
        return states;
    }

    public override string ToString()
    {
        return Name + " - " + CellPosition;
    }
}