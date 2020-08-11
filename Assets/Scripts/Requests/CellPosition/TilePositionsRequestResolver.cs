using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePositionsRequestResolver : RequestResolver<List<Vector2>>
{
    [SerializeField] private CellsPositionsRequest request;
    [SerializeField] private Tilemap[] tilemaps;
    [SerializeField] private Grid grid;

    public override Request<List<Vector2>> Request => request;

    public override bool Resolve(ParamsObject requestParams, out List<Vector2> tilePositions)
    {
        tilePositions = new List<Vector2>();
        TileBase neededTile = requestParams.GetParam<TileBase>();

        foreach (Tilemap tilemap in tilemaps)
        {
            for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
            {
                for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
                {
                    Vector3Int localPlace = (new Vector3Int(x, y, (int)tilemap.transform.position.y));
                    
                    if (tilemap.GetTile(localPlace) == neededTile)
                    {
                        tilePositions.Add(tilemap.CellToWorld(localPlace));
                    }
                }
            }
        }
        return tilePositions.Count > 0;
    }
}
