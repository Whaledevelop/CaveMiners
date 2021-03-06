﻿using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Правило для генерации одного тайла
/// </summary>
[CreateAssetMenu(fileName = "GenerateEverywhereRule", menuName = "LevelGeneratorRules/GenerateEverywhereRule")]
public class GenerateSingleRule : GenerateRule
{
    [SerializeField] private GenerativeTilemapsSet tilemapsSet;
    [SerializeField] private GenerativeTilemapCode tilemapCode;
    [SerializeField] private TileBase tile;

    private GenerativeTilemap generativeTilemap;

    public override bool HandlePosition(int x, int y)
    {
        if (CheckPosition(x, y))
        {
            if (generativeTilemap == null)
                generativeTilemap = tilemapsSet.FindByCode(tilemapCode);
            generativeTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            return true;
        }
        else
            return false;
    }

    protected virtual bool CheckPosition(int x, int y) => true;
}