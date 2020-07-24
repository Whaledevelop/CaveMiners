using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StateAction
{
    public CharacterStateData state;
    public Vector2 prevCellPosition;
    public Vector2 axisFromPrevCell;

    public StateAction prevPoint;
    public int nestLevel = 1;

    public List<int> positionInPath = new List<int>();
    public List<StateAction> nextActionsPoints = new List<StateAction>();
    public string Name => state.stateName;
    public int Priority => state.actionPriority;
    public Vector2 CellPosition => prevCellPosition + axisFromPrevCell;
   
    public Vector2 NextCellToCharacterPosition; // Для гизмо

    public StateAction(CharacterStateData state, Vector2 prevCellPosition, Vector2 axisFromPrevCell)
    {
        this.state = state;
        this.prevCellPosition = prevCellPosition;
        this.axisFromPrevCell = axisFromPrevCell;
    }   

    public void GetPointWithAllPrevs(ref List<StateAction> nextPoints)
    {
        nextPoints.Add(this);
        if (prevPoint != null)
            prevPoint.GetPointWithAllPrevs(ref nextPoints);
    }

    public void AddPathsToCertainPositionInTree(List<int> parentPositionInPath, List<StateAction> paths)
    {
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
            foreach (StateAction availablePath in nextActionsPoints)
            {
                availablePath.AddPathsToCertainPositionInTree(parentPositionInPath, paths);
            }
        }
    }

    public List<StateAction> GetAllStatesWithNestLevel(int nestLevel)
    {
        List<StateAction> states = new List<StateAction>();
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
        return states;
    }

    public override string ToString()
    {
        return Name + " - " + CellPosition;
    }
}