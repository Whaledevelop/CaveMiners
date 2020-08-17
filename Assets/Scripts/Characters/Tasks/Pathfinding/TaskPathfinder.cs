using UnityEngine;
using System.Collections.Generic;

public abstract class TaskPathfinder : ScriptableObject
{
    public abstract List<PathPoint> FindPath(Vector2 fromPosition, Vector2 toPosition, int taskLayer);
}