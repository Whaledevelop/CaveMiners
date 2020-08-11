using UnityEngine;

public abstract class GenerateRule : ScriptableObject
{
    public bool isSingleOnCell;

    public abstract bool HandlePosition(int x, int y, RangeInt xRange, RangeInt yRange);
}