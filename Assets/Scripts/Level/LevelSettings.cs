using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "ScriptableObjects/LevelSettings")]
public class LevelSettings : ScriptableObject
{
    /// <summary>
    /// Количество для определенного правила создания тайлов. Обновляемое от уровня к уровню
    /// </summary>
    [Serializable]
    public class GeneratedTilesCount
    {
        public GenerateRule generateRule;
        public int initialCount;
        public int levelUpPlus;

        private int count;
        public int Count
        {
            get
            {
                if (count == 0)
                    count = initialCount;
                return count;
            }
        }

        public GeneratedTilesCount(GenerateRule generateRule, int initialCount)
        {
            this.generateRule = generateRule;
            this.initialCount = count;
        }

        public void Upgrade()
        {
            count += levelUpPlus;
        }
    }

    [SerializeField] private List<GeneratedTilesCount> tilesCount = new List<GeneratedTilesCount>();

    public int startMoney;

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

    /// <summary>
    /// При обновлении настроек меняем количество генерируемых тайлов и размеры уровня
    /// </summary>
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

    /// <summary>
    /// Устанавливаем нужное количество тайлов (если на момент инициализации неизвестно, сколько будет)
    /// </summary>
    public void SetTilesCount(GenerateRule rule, int initialCount)
    {
        GeneratedTilesCount tileCount = tilesCount.Find(tile => tile.generateRule == rule);
        if (tileCount == null)
            tilesCount.Add(new GeneratedTilesCount(rule, initialCount));
        else
            tileCount.initialCount = initialCount;
    }

    public int GetTilesCount(GenerateRule generateTileRule)
    {
        GeneratedTilesCount countObj = tilesCount.Find(tileCount => tileCount.generateRule == generateTileRule);
        return countObj != null ? countObj.Count : 0;
    }
}
