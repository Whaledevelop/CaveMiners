using UnityEngine;
using UnityEngine.Tilemaps;


public abstract class GenerateSingleRule : GenerateRule
{
    [SerializeField] private GenerativeTilemapsSet tilemapsSet;
    [SerializeField] private GenerativeTilemapCode tilemapCode;
    [SerializeField] private TileBase tile;
    [SerializeField] private bool isSingleOnCell;

    private GenerativeTilemap generativeTilemap;

    public override bool HandlePosition(int x, int y, RangeInt levelXRange, RangeInt levelYRange)
    {
        if (CheckPosition(x, y, levelXRange, levelYRange))
        {
            if (generativeTilemap == null)
                generativeTilemap = tilemapsSet.FindByCode(tilemapCode);
            generativeTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            return isSingleOnCell;
        }
        else
            return false;
    }

    protected abstract bool CheckPosition(int x, int y, RangeInt levelXRange, RangeInt levelYRange);
}