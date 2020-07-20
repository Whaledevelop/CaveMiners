using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DigRequestResolver : RequestResolver
{
    [SerializeField] private List<Tilemap> diggableTilemaps = new List<Tilemap>();

    [SerializeField] private Grid grid;

    public override bool Resolve(object[] requestParams, out object[] resolveParams)
    {
        Vector3 diggerPosition = (Vector3)requestParams[0];
        Vector2Int digDirection = (Vector2Int)requestParams[1];

        if (digDirection.x != 0)
        {
            Vector2 digPosition = new Vector2(diggerPosition.x + digDirection.x * grid.cellSize.x, diggerPosition.y);

            Vector3Int digPositionInt = Vector3Int.FloorToInt(digPosition);

            Vector3Int digCellPositionInt = grid.WorldToCell(digPositionInt);

            foreach (Tilemap digableTilemap in diggableTilemaps)
            {
                TileBase digTile = digableTilemap.GetTile(digCellPositionInt);
                if (digTile != null)
                {
                    resolveParams = new object[1] { digCellPositionInt };
                    return true;
                }
                    
            }
        }
        resolveParams = null;
        return false;
    }
}
