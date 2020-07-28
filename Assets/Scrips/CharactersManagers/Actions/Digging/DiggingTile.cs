using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiggingTile
{
    public Tilemap tilemap;
    public List<TileData> tilesSet;
    public Vector3Int cellIntPosition;
    public List<CharacterActionData> activeActions;
    public float HP;

    public TileBase activeTile;

    public Action onDigged;

    public DiggingTile(List<TileData> tilesSet, TileBase activeTile, Tilemap tilemap, Vector3Int cellIntPosition, CharacterActionData firstActionData, int initialHP)
    {
        this.tilesSet = tilesSet;
        this.activeTile = activeTile;
        this.tilemap = tilemap;
        this.cellIntPosition = cellIntPosition;
        activeActions = new List<CharacterActionData>() { firstActionData };
        HP = initialHP;
    }

    public void Dig(CharacterActionData actionData)
    {
        HP -= actionData.stateSkill;
        if (HP <= 0)
        {
            tilemap.SetTile(cellIntPosition, null);
            foreach (CharacterActionData action in activeActions)
            {
                action.taskManager.OnExecuteState();
            }
            onDigged?.Invoke();
        }
        else
        {
            TileData neededTile = tilesSet.LastOrDefault(tileData => tileData.HP >= HP);
            if (neededTile != null && neededTile.tile != activeTile)
            {
                tilemap.SetTile(cellIntPosition, neededTile.tile);
                activeTile = neededTile.tile;
            }
        }
    }
}
