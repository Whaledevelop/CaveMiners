﻿using UnityEngine;
using System.Collections.Generic;

public abstract class TaskPathfinder : ScriptableObject
{
    public abstract List<StateAction> FindPath(Vector2 fromPosition, GameObject toObject, Vector2 toPosition);
}