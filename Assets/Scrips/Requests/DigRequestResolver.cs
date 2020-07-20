using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DigRequestResolver : RequestResolver
{
    [SerializeField] private List<Tilemap> diggableTilemaps = new List<Tilemap>();

    [SerializeField] private Grid grid;

    public override ParamsObject Resolve(object[] requestParams)
    {
        Vector2 diggerPosition = (Vector2)requestParams[0];
        Vector2 digDirection = (Vector2)requestParams[1];

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
                    return new ParamsObject(digCellPositionInt);
                }
                    
            }
        }
        return null;
    }
}
