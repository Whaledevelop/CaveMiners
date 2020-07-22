using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CellLayoutRequest", menuName = "Requests/CellLayoutRequest")]
public class CellLayoutRequest : Request<LayerMask> { }

public class CellLayoutRequestResolver : RequestResolver<LayerMask>
{
    [SerializeField] private CellLayoutRequest cellLayoutRequest;
    [SerializeField] private Tilemap[] tilemaps;
    [SerializeField] private Grid grid;

    public override Request<LayerMask> Request => cellLayoutRequest;

    public override bool Resolve(ParamsObject requestParams, out LayerMask resolveParams)
    {
        Vector2 cellWorldPosition = requestParams.GetParam<Vector2>();
        Vector3Int cellGridPosition = grid.WorldToCell(cellWorldPosition);
        Tilemap tilemap = tilemaps.FirstOrDefault(t => t.HasTile(cellGridPosition));
        if (tilemap != null)
        {
            resolveParams = tilemap.gameObject.layer;
            return true;
        }
        else
        {
            resolveParams = default;
            return false;
        }             
    }
}
