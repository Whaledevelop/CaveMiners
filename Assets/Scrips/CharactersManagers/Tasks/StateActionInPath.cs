using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateActionInPath : StateAction
{
    public StateActionInPath prevStatePath;

    public List<StateActionInPath> availablePaths = new List<StateActionInPath>();

    public int nestLevel = 0;


    public StateActionInPath(CharacterStateData state, Vector2 fromCellPosition, Vector2 actionAxis) : base(state, fromCellPosition, actionAxis) {}

    public void GetAllStatesWithNestLevel(ref List<StateActionInPath> states, int nestLevel)
    {
        if (this.nestLevel == nestLevel)
        {
            states.Add(this);
        }            
        else if (this.nestLevel < nestLevel)
        {
            foreach(StateActionInPath innerPath in availablePaths)
            {
                GetAllStatesWithNestLevel(ref states, nestLevel + 1);
            }    
        }
    }

    public void AddPathsFromCell(List<StateActionInPath> taskPathsCell)
    {
        for(int i = 0; i < taskPathsCell.Count; i++)
        {
            taskPathsCell[i].nestLevel = nestLevel + 1;
            taskPathsCell[i].prevStatePath = this;
        }
        availablePaths.AddRange(taskPathsCell);
    }

    public void AddPathFromCell(StateActionInPath taskPathCell)
    {
        taskPathCell.nestLevel = nestLevel + 1;
        taskPathCell.prevStatePath = this;
        availablePaths.Add(taskPathCell);
    }

    //private string[] colors = new string[5] { "white", "yellow", "green", "red", "blue" };
    //public void LogPathWithAllInner(int nestingLevel, int numberInInners = 0)
    //{
    //    Debugger.Log(ToString() + ", level " + nestingLevel + " (" + numberInInners + ")", colors[numberInInners]);
    //    for(int i = 0; i < availablePaths.Count; i++)
    //    {
    //        int pathNumber = numberInInners == 0 ? i+1 : numberInInners;
    //        availablePaths[i].LogPathWithAllInner(nestingLevel+1, pathNumber);
    //    }
    //}

    public List<Vector2> GetVector2Points(List<Vector2> vector2s)
    {
        if (!vector2s.Contains(ToCellPosition))
            vector2s.Add(ToCellPosition);
        foreach (StateActionInPath innerPath in availablePaths)
        {
            vector2s = innerPath.GetVector2Points(vector2s);
        }
        return vector2s;
    }
}
