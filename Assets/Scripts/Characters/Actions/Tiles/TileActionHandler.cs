using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileActionHandler : MonoBehaviour, IIteractiveActionHandler
{
    [SerializeField] private Tilemap initialTilemap;

    [SerializeField] private Tilemap finalTilemap;

    [SerializeField] private TileBase finalTile;

    [SerializeField] private Grid grid;

    [SerializeField] private List<TileDependOnHP> tilesSet = new List<TileDependOnHP>();

    [SerializeField] private int initialHP;

    private List<ActionTile> actionTiles = new List<ActionTile>();

    public void OnStartAction(CharacterActionData actionData)
    {
        Vector3Int digCellPositionInt = grid.WorldToCell(Vector3Int.FloorToInt(actionData.endPosition));

        TileBase digTile = initialTilemap.GetTile(digCellPositionInt);
        if (digTile != null)
        {
            TileCell groundTileData = new TileCell(digTile, initialTilemap, digCellPositionInt);
            TileCell tunnelTileData = new TileCell(finalTile, finalTilemap, digCellPositionInt);
            ActionTile diggingTile = new ActionTile(tilesSet, groundTileData, tunnelTileData, actionData, initialHP);
            diggingTile.OnWorkedOut += () => actionTiles.Remove(diggingTile);
            actionTiles.Add(diggingTile);
        }
    }

    public void OnIteration(CharacterActionData actionData)
    {
        ActionTile digTile = actionTiles.Find(diggingTile => diggingTile.activeActions.Contains(actionData));
        if (digTile != null)
        {
            digTile.OnIteration(actionData);
        }
    }

    public void OnStopAction(CharacterActionData actionData)
    {
        ActionTile digTile = actionTiles.Find(tile => tile.activeActions.Contains(actionData));
        if (digTile != null)
        {
            digTile.RemoveActiveAction(actionData);
        }
    }

    public void OnEndAction(CharacterActionData actionData)
    {
        //throw new NotImplementedException();
    }
}
