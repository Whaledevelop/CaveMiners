using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePositionRequestResolver : RequestResolver<Vector2>
{
    [SerializeField] private CellPositionRequest request;
    [SerializeField] private Tilemap[] tilemaps;
    [SerializeField] private Grid grid;

    public override Request<Vector2> Request => request;

    public override bool Resolve(ParamsObject requestParams, out Vector2 tilePosition)
    {        
        TileBase neededTile = requestParams.GetParam<TileBase>();

        foreach (Tilemap tilemap in tilemaps)
        {
            for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++)
            {
                for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++)
                {
                    Vector3Int localPlace = (new Vector3Int(n, p, (int)tilemap.transform.position.y));
                    
                    if (tilemap.GetTile(localPlace) == neededTile)
                    {
                        tilePosition = tilemap.CellToWorld(localPlace);
                        return true;
                    }
                }
            }
        }
        tilePosition = default;
        return false;
    }
}
