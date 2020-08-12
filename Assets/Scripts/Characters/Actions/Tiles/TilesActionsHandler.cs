using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TilesActionsHandler : MonoBehaviour
{
    [SerializeField] private List<TileAction> tilesActions;
    [SerializeField] private Grid grid;
    [SerializeField] private CharacterActionGameEvent tileWorkedOutEvent;

    private List<TileAction> activeTiles = new List<TileAction>();

    public void OnTileAction(CharacterAction actionData)
    {        
        TileAction activeTile = activeTiles.Find(tile =>
        {
            return tile.actionData.endPosition == actionData.endPosition;
        });

        if (activeTile == null)
        {
            TileAction newActiveTile = tilesActions.Find(tile => tile.State == actionData.stateData);       
            if (newActiveTile != null)
            {
                TileAction instantiatedTile = Instantiate(newActiveTile);
                instantiatedTile.Init(actionData, grid.WorldToCell(Vector3Int.FloorToInt(actionData.endPosition)));
                instantiatedTile.OnWorkedOut += OnTileWorkedOut;
                activeTiles.Add(instantiatedTile);
            }
        }

        if (activeTile != null)
        {
            activeTile.Damage(actionData.stateSkill);
        }
    }

    public void OnTileWorkedOut(TileAction tileAction)
    {
        activeTiles.Remove(tileAction);
        tileWorkedOutEvent.Raise(tileAction.actionData);

    }
}
