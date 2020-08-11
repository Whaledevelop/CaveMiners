using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class GenerateRule : ScriptableObject
{
    [SerializeField] private GenerativeTilemapsSet tilemapsSet;
    [SerializeField] private GenerativeTilemapCode tilemapCode;
    [SerializeField] private TileBase tile;
    [SerializeField] private bool isSingleOnCell;

    private GenerativeTilemap generativeTilemap; 

    public bool HandlePosition(int x, int y, RangeInt xRange, RangeInt yRange)
    {
        if (DefineIfTileObservesRule(x, y, xRange, yRange))
        {
            if (generativeTilemap == null)
                generativeTilemap = tilemapsSet.FindByCode(tilemapCode);

            generativeTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            return isSingleOnCell;
        }
        else
            return false;
    }

    protected abstract bool DefineIfTileObservesRule(int x, int y, RangeInt xRange, RangeInt yRange);
}