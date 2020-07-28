using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IfOnSameCellRequestResolver : RequestResolver
{
    [SerializeField] private Grid grid;
    public override bool Resolve(ParamsObject requestParams)
    {
        List<Vector2> worldPositions = requestParams.GetAllOfType<Vector2>();

        Vector3Int firstElementCellPosition = grid.WorldToCell(worldPositions[0]);

        return worldPositions.All(worldPosition => grid.WorldToCell(worldPosition) == firstElementCellPosition);
    }
}