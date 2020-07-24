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
   
    public Vector2 NextCellToCharacterPosition; // Для гизмо

    public StateActionPoint(CharacterStateData state, Vector2 prevCellPosition, Vector2 axisFromPrevCell)
    {
        this.state = state;
        this.prevCellPosition = prevCellPosition;
        this.axisFromPrevCell = axisFromPrevCell;
    }   

    public void GetPointWithAllPrevs(ref List<StateActionPoint> nextPoints)
    {
        nextPoints.Add(this);
        if (prevPoint != null)
            prevPoint.GetPointWithAllPrevs(ref nextPoints);
    }

    public void AddPathsToCertainPositionInTree(List<int> parentPositionInPath, List<StateActionPoint> paths)
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
            foreach (StateActionPoint availablePath in nextActionsPoints)
            {
                availablePath.AddPathsToCertainPositionInTree(parentPositionInPath, paths);
            }
        }
    }

    public List<StateActionPoint> GetAllStatesWithNestLevel(int nestLevel)
    {
        List<StateActionPoint> states = new List<StateActionPoint>();
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