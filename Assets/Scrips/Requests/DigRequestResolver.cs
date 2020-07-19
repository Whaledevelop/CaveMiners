using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct TileData
{
    public Vector3Int position;
    public Tilemap tilemap;
    public TileBase tile;

    public TileData(Vector3Int position, Tilemap tilemap, TileBase tile)
    {
        this.position = position;
        this.tilemap = tilemap;
        this.tile = tile;
    }
}

[CreateAssetMenu(fileName = "TileRequest", menuName = "ScriptableObjects/TileRequest")]
public class TileRequest : Request<TileData> { }

public class DigRequestResolver : RequestResolver<TileData>
{
    [SerializeField] private List<Tilemap> diggableTilemaps = new List<Tilemap>();

    [SerializeField] private Grid grid;

    [SerializeField] private TileRequest tileRequest;
    public override Request<TileData> request => tileRequest;

    public override TileData Resolve(object[] requestParams)
    {
        Vector2 diggerPosition = (Vector3)requestParams[0];
        Vector2Int digDirection = (Vector2Int)requestParams[1];
        if (digDirection.x != 0)
        {
            Vector2 digPosition = new Vector2(diggerPosition.x + digDirection.x * grid.cellSize.x, digDirection.y);

            // Определить координаты, где собирается копать. При этом учесть угол поворота (пока влево вправо)
            Vector3Int digPositionInt = grid.WorldToCell(Vector3Int.FloorToInt(digPosition));

            foreach (Tilemap digableTilemap in diggableTilemaps)
            {
                TileBase digTile = digableTilemap.GetTile(digPositionInt);
                if (digTile != null)
                    return new TileData(digPositionInt, digableTilemap, digTile);
            }
        }
        return default;

    }
}
