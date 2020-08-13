using UnityEngine;

public abstract class GenerateRule : ScriptableObject
{
    public bool isSingleOnCell;

    protected RangeInt levelXRange;
    protected RangeInt levelYRange;

    public virtual void Init(LevelSettings levelSettings)
    {
        levelXRange = levelSettings.xLevelSizeRange;
        levelYRange = levelSettings.yLevelSizeRange;
    }

    public abstract bool HandlePosition(int x, int y);
}