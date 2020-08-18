using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Правило для генарции в рандомном месте в определенной области
/// </summary>
[CreateAssetMenu(fileName = "GenerateInAreaRule", menuName = "LevelGeneratorRules/GenerateInAreaRule")]
public class GenerateInAreaRule : GenerateSingleRule
{
    [Header("Сколько экземпляров")]
    [SerializeField] private int defaultInstanceCount = 1;

    [Header("Размер одного экземпляра")]
    [SerializeField] private Vector2Int instanceDirection;
    [SerializeField] private bool isInstanceRect;

    [Header("Может ли превышать разрешенную область (если размер больше)")]
    [SerializeField] private bool isExpandable;

    [HideInInspector] public Range xArea;
    [HideInInspector] public Range yArea;

    [NonSerialized] private List<TilesGroup> tilesGroups = new List<TilesGroup>();

    [NonSerialized] private int instancesCount;

    public override void Init(LevelSettings levelSettings)
    {
        tilesGroups.Clear();
        base.Init(levelSettings);
        int tilesCount = levelSettings.GetTilesCount(this);
        instancesCount = tilesCount == 0 ? defaultInstanceCount : tilesCount;

        for (int i = 0; i < instancesCount; i++)
        {
            int randomX = (int)Mathf.Round(levelXRange.Interval * xArea.Random);
            int randomY = (int)Mathf.Round(levelYRange.Interval * yArea.Random);
            TilesGroup tilesGroup = new TilesGroup(TilesGroup.Orientation.LowerLeft, new Vector2Int(randomX, randomY), instanceDirection, isInstanceRect);
            tilesGroup.Init(levelXRange, levelYRange);
            tilesGroups.Add(tilesGroup);
        }
    }

    protected override bool CheckPosition(int x, int y)
    {
        foreach(TilesGroup tilesGroup in tilesGroups)
        {
            if (tilesGroup.CheckIfGroupPosition(x, y))
                return true;
        }
        return false;

    }
}