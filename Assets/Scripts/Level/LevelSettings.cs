using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "ScriptableObjects/LevelSettings")]
public class LevelSettings : ScriptableObject
{
    public List<GeneratedTilesCount> tilesCount = new List<GeneratedTilesCount>();

    #region Размеры уровня

    [SerializeField] private RangeInt initXLevelSizeRange;
    [SerializeField] private RangeInt xLevelSizeUpgrade;
    [SerializeField] private RangeInt initYLevelSizeRange;
    [SerializeField] private RangeInt yLevelSizeUpgrade;

    private RangeInt xLevelSizeRange;
    
    public RangeInt XLevelSizeRange
    {
        get
        {
            if (xLevelSizeRange.IsDefault)
                xLevelSizeRange = initXLevelSizeRange;
            return xLevelSizeRange;
        }
    }

    private RangeInt yLevelSizeRange;

    public RangeInt YLevelSizeRange
    {
        get
        {
            if (yLevelSizeRange.IsDefault)
                yLevelSizeRange = initYLevelSizeRange;
            return yLevelSizeRange;
        }
    }

    #endregion

    [HideInInspector] public int UpdateLevel = 1;

    public void Upgrade()
    {
        foreach(GeneratedTilesCount tileCount in tilesCount)
        {
            tileCount.Upgrade();
        }
        xLevelSizeRange = new RangeInt(xLevelSizeRange.from + xLevelSizeUpgrade.from, xLevelSizeRange.to + xLevelSizeUpgrade.to);
        yLevelSizeRange = new RangeInt(yLevelSizeRange.from + yLevelSizeUpgrade.from, yLevelSizeRange.to + yLevelSizeUpgrade.to);

        UpdateLevel++;
    }

    public void SetTilesCount(GenerateRule rule, int initialCount)
    {
        GeneratedTilesCount tileCount = tilesCount.Find(tile => tile.generateRule == rule);
        if (tileCount == null)
            tilesCount.Add(new GeneratedTilesCount(rule, initialCount));
        else
            tileCount.initialCount = initialCount;
    }
}
