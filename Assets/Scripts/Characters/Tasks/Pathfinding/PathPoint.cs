using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Определенная промежуточная точка в пути
/// </summary>
public class PathPoint
{    
    public CharacterActionState state;
    public Vector2 prevCellPosition;
    public Vector2 axisFromPrevCell;

    public PathPoint prevPoint;

    public int nestLevel = 1;
    // Позиция в пути, где индекс - это глубина вложенности, а значение - это индекс среди точек данного уровня
    public List<int> positionInPath = new List<int>();
    // 
    public List<PathPoint> nextPoints = new List<PathPoint>();
    public Vector2 NextCellToCharacterPosition; // Для гизмо

    public string Name => state.stateName;
    public int Priority => state.priority;
    public Vector2 CellPosition => prevCellPosition + axisFromPrevCell;
   


    public Vector2 AxisToNextCell => NextCellToCharacterPosition - CellPosition;

    public PathPoint(CharacterActionState state, Vector2 prevCellPosition, Vector2 axisFromPrevCell)
    {
        this.state = state;
        this.prevCellPosition = prevCellPosition;
        this.axisFromPrevCell = axisFromPrevCell;
    }   

    public void GetPointWithAllPrevs(ref List<PathPoint> nextPoints)
    {
        nextPoints.Add(this);
        if (prevPoint != null)
            prevPoint.GetPointWithAllPrevs(ref nextPoints);
    }

    public void AddPathsToCertainPositionInTree(List<int> parentPositionInPath, List<PathPoint> paths)
    {
        if (positionInPath == parentPositionInPath) // Если это батя
        {
            for (int i = 0; i < paths.Count; i++)
            {
                paths[i].positionInPath = new List<int>(positionInPath) { i };
                paths[i].nestLevel = nestLevel + 1;
                paths[i].prevPoint = this;
            }
            nextPoints.AddRange(paths);
        }
        else if (positionInPath.Count < parentPositionInPath.Count) // Проверяем предка
        {
            for(int i = 0; i < positionInPath.Count; i++)
            {
                if (positionInPath[i] != parentPositionInPath[i])
                    return;
            }
            foreach (PathPoint availablePath in nextPoints)
            {
                availablePath.AddPathsToCertainPositionInTree(parentPositionInPath, paths);
            }
        }
    }

    public List<PathPoint> GetAllStatesWithNestLevel(int nestLevel)
    {
        List<PathPoint> states = new List<PathPoint>();
        if (this.nestLevel == nestLevel)
        {
            states.Add(this);
        }
        else if (this.nestLevel < nestLevel)
        {
            for (int i = 0; i < nextPoints.Count; i++)
            {
                states.AddRange(nextPoints[i].GetAllStatesWithNestLevel(nestLevel));
            }
        }
        return states;
    }

    public override string ToString()
    {
        return Name + " - " + CellPosition;
    }
}