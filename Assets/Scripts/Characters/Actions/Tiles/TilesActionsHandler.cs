using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TilesActionsHandler : MonoBehaviour
{
    [SerializeField] private TileAction[] tilesActions;
    [SerializeField] private Grid grid;
    [SerializeField] private CharacterActionGameEvent tileWorkedOutEvent;

    private List<TileAction> activeTiles = new List<TileAction>();

    public void OnTileAction(CharacterAction actionData)
    {
        Vector3Int cellPosition = grid.WorldToCell(Vector3Int.FloorToInt(actionData.endPosition));
        actionData.endCellPosition = cellPosition;

        TileAction activeTile = activeTiles.Find(tile => tile.CellPosition == cellPosition);

        if (activeTile == null)
        {
            activeTile = tilesActions.FirstOrDefault(tile => tile.actionState == actionData.stateData);

            if (activeTile != null)
            {
                activeTile.Init(actionData);
                activeTile.OnWorkedOut += tileWorkedOutEvent.Raise;
                activeTiles.Add(activeTile);
            }
        }

        if (activeTile != null)
        {
            activeTile.Damage(actionData.stateSkill);
        }
    }
}
