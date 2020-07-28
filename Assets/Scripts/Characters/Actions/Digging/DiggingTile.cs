using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiggingTile
{
    public TileCell groundTileCell;
    public TileCell tunnelTileCell;

    public List<TileDependOnHP> tilesSet;
    public List<CharacterActionData> activeActions;
    public float HP;
    public Action onDigged;

    public DiggingTile(List<TileDependOnHP> tilesSet, TileCell groundTileCell, TileCell tunnelTileCell, CharacterActionData firstActionData, int initialHP)
    {
        this.tilesSet = tilesSet;
        this.groundTileCell = groundTileCell;
        this.tunnelTileCell = tunnelTileCell;
        activeActions = new List<CharacterActionData>() { firstActionData };
        HP = initialHP;
    }

    public void Dig(CharacterActionData actionData)
    {
        HP -= actionData.stateSkill;
        if (HP <= 0)
        {
            groundTileCell.SetTile(null);
            tunnelTileCell.SetTile(tunnelTileCell.tile);
            foreach (CharacterActionData action in activeActions)
            {
                action.taskManager.OnExecuteState();
            }
            onDigged?.Invoke();
        }
        else
        {
            TileDependOnHP neededTile = tilesSet.LastOrDefault(tileData => tileData.HP >= HP);
            if (neededTile != null && neededTile.tile != groundTileCell.tile)
            {
                groundTileCell.SetTile(neededTile.tile);
            }
        }
    }
}
