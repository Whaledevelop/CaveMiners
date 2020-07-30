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

        ActionTile activeTile = actionTiles.Find(tile => tile.initialTileCell.cellPosition == digCellPositionInt);

        if (activeTile == null)
        {
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
        else
        {
            activeTile.activeActions.Add(actionData);
        }
    }

    public void OnIteration(CharacterActionData actionData)
    {
        ActionTile actionTile = actionTiles.Find(tile => tile.activeActions.Contains(actionData));
        if (actionTile != null)
        {
            actionTile.OnIteration(actionData);
        }
    }

    public void OnStopAction(CharacterActionData actionData)
    {
        foreach(ActionTile actionTile in actionTiles)
        {
            int removeActionIndex = -1;
            for(int i = 0; i < actionTile.activeActions.Count; i++)
            {
                if (actionTile.activeActions[i].taskManager == actionData.taskManager)
                    removeActionIndex = i;
            }
            if (removeActionIndex != -1)
                actionTile.activeActions.RemoveAt(removeActionIndex);
        }
    }

    public void OnEndAction(CharacterActionData actionData)
    {
        //throw new NotImplementedException();
    }
}
