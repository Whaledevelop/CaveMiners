using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTaskPoint
{
    public CharacterActionState stateData;
    public Vector2 prevCellPosition;
    public Vector2 axisFromPrevCell;

    public CharacterTaskPoint prevPoint;
    public int nestLevel = 1;

    public List<int> positionInPath = new List<int>();
    public List<CharacterTaskPoint> nextActionsPoints = new List<CharacterTaskPoint>();
    public string Name => stateData.stateName;
    public int Priority => stateData.actionPriority;
    public Vector2 CellPosition => prevCellPosition + axisFromPrevCell;
   
    public Vector2 NextCellToCharacterPosition; // Для гизмо

    public Vector2 AxisToNextCell => NextCellToCharacterPosition - CellPosition;

    public CharacterTaskPoint(CharacterActionState stateData, Vector2 prevCellPosition, Vector2 axisFromPrevCell)
    {
        this.stateData = stateData;
        this.prevCellPosition = prevCellPosition;
        this.axisFromPrevCell = axisFromPrevCell;
    }   

    public void GetPointWithAllPrevs(ref List<CharacterTaskPoint> nextPoints)
    {
        nextPoints.Add(this);
        if (prevPoint != null)
            prevPoint.GetPointWithAllPrevs(ref nextPoints);
    }

    public void AddPathsToCertainPositionInTree(List<int> parentPositionInPath, List<CharacterTaskPoint> paths)
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
                if (positionInPath[i] != parentPositionInPath[i])
                    return;
            }
            foreach (CharacterTaskPoint availablePath in nextActionsPoints)
            {
                availablePath.AddPathsToCertainPositionInTree(parentPositionInPath, paths);
            }
        }
    }

    public List<CharacterTaskPoint> GetAllStatesWithNestLevel(int nestLevel)
    {
        List<CharacterTaskPoint> states = new List<CharacterTaskPoint>();
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