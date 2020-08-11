using UnityEngine;

public abstract class GenerateRule : ScriptableObject
{
    public abstract bool HandlePosition(int x, int y, RangeInt xRange, RangeInt yRange);
}