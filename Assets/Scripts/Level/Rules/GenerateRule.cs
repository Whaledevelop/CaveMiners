using UnityEngine;

/// <summary>
/// Правило для генерации объектов на сцене. 
/// </summary>
public abstract class GenerateRule : ScriptableObject
{
    public bool isSingleOnCell;

    protected RangeInt levelXRange;
    protected RangeInt levelYRange;

    public virtual void Init(LevelSettings levelSettings)
    {
        levelXRange = levelSettings.XLevelSizeRange;
        levelYRange = levelSettings.YLevelSizeRange;
    }

    public abstract bool HandlePosition(int x, int y);
}