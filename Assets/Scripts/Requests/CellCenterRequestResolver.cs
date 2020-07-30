using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CellPositionRequest", menuName = "Requests/CellPositionRequest")]
public class CellPositionRequest : Request<Vector2> { }

public class CellCenterRequestResolver : RequestResolver<Vector2>
{
    [SerializeField] private CellPositionRequest request;
    [SerializeField] private Grid grid;

    public override Request<Vector2> Request => request;

    public override bool Resolve(ParamsObject requestParams, out Vector2 cellCenter)
    {
        Vector2 cellWorldPosition = requestParams.GetParam<Vector2>();
        Vector3Int cellGridPosition = grid.WorldToCell(cellWorldPosition);
        float xCenter = Mathf.Floor(cellGridPosition.x) + grid.cellSize.x / 2;
        float yCenter = Mathf.Floor(cellGridPosition.y) + grid.cellSize.y / 2;
        cellCenter = new Vector2(xCenter, yCenter);
        return true;
    }
}
