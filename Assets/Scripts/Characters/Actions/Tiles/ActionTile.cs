using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ActionTile
{
    public TileCell initialTileCell;
    public TileCell finalTileCell;

    public List<TileDependOnHP> tilesSet;
    public List<CharacterActionData> activeActions;
    public float HP;
    public Action OnWorkedOut;

    public ActionTile(List<TileDependOnHP> tilesSet, TileCell initialTileCell, TileCell finalTileCell, CharacterActionData firstActionData, int initialHP)
    {
        this.tilesSet = tilesSet;
        this.initialTileCell = initialTileCell;
        this.finalTileCell = finalTileCell;
        activeActions = new List<CharacterActionData>() { firstActionData };
        HP = initialHP;
    }

    public void OnIteration(CharacterActionData actionData)
    {
        Debug.Log("OnIteration " + HP);
        HP -= actionData.stateSkill;
        if (HP <= 0)
        {
            OnEndHP();
        }
        else
        {
            OnChangeHP();
        }
    }

    public virtual void OnEndHP()
    {
        initialTileCell.SetTile(null);
        finalTileCell.SetTile(finalTileCell.tile);
        OnWorkedOut?.Invoke();
        foreach (CharacterActionData action in activeActions)
        {
            action.OnExecute();
        }        
    }

    public virtual void OnChangeHP()
    {
        TileDependOnHP neededTile = tilesSet.LastOrDefault(tileData => tileData.HP >= HP);
        if (neededTile != null && neededTile.tile != initialTileCell.tile)
        {
            initialTileCell.SetTile(neededTile.tile);
        }
    }

    public void RemoveActiveAction(CharacterActionData actionData)
    {
        activeActions.Remove(actionData);
    }
}
