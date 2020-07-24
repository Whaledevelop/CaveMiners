using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//public class StateActionPointInPath : StateActionPoint
//{

//    //public void AddPathsFromCell(List<StateActionPointInPath> taskPathsCell)
//    //{
//    //    for(int i = 0; i < taskPathsCell.Count; i++)
//    //    {         
//    //        taskPathsCell[i].positionInPath = positionInPath;
//    //        taskPathsCell[i].positionInPath.Add(i);
//    //    }
//    //    availablePaths.AddRange(taskPathsCell);
//    //}

//    //public void AddPathFromCell(StateActionPointInPath taskPathCell)
//    //{
//    //    taskPathCell.positionInPath = positionInPath;
//    //    taskPathCell.positionInPath.Add(availablePaths.Count);
//    //    availablePaths.Add(taskPathCell);
//    //}

//    //private string[] colors = new string[5] { "white", "yellow", "green", "red", "blue" };
//    //public void LogPathWithAllInner(int nestingLevel, int numberInInners = 0)
//    //{
//    //    Debugger.Log(ToString() + ", level " + nestingLevel + " (" + numberInInners + ")", colors[numberInInners]);
//    //    for(int i = 0; i < availablePaths.Count; i++)
//    //    {
//    //        int pathNumber = numberInInners == 0 ? i+1 : numberInInners;
//    //        availablePaths[i].LogPathWithAllInner(nestingLevel+1, pathNumber);
//    //    }
//    //}

//    //public List<Vector2> GetVector2Points(List<Vector2> vector2s)
//    //{
//    //    if (!vector2s.Contains(CellPosition))
//    //        vector2s.Add(CellPosition);
//    //    foreach (StateActionPointInPath innerPath in availablePaths)
//    //    {
//    //        vector2s = innerPath.GetVector2Points(vector2s);
//    //    }
//    //    return vector2s;
//    //}
//}
