using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

[System.Serializable] public struct TileCell
{
    public TileBase tile;
    public Tilemap tilemap;
    public Vector3Int cellPosition;

    public TileCell(TileBase tile, Tilemap tilemap, Vector3Int cellPosition)
    {
        this.tile = tile;
        this.tilemap = tilemap;
        this.cellPosition = cellPosition;
    }

    public void SetTile(TileBase tile)
    {
        tilemap.SetTile(cellPosition, tile);
        this.tile = tile;
    }
}
