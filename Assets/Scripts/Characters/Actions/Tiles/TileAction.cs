using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class TileAction
{
    public CharacterActionState actionState;
    public Tilemap initialTilemap;
    public Tilemap destroyedTilemap;
    public List<TileDependOnHP> tilesSet;
    public float initialHP;

    public Action<CharacterAction> OnWorkedOut;

    private TileBase currentTile;
    private float HP;

    [HideInInspector] public CharacterAction actionData;
    [HideInInspector] public Vector3Int CellPosition => actionData.endCellPosition;

    public void Init(CharacterAction actionData)
    {
        this.actionData = actionData;
        HP = initialHP;
    }

    public void Damage(float damage)
    {
        HP -= damage;

        TileDependOnHP neededTile = tilesSet.LastOrDefault(tileData => tileData.HP >= HP);
        if (neededTile != null && neededTile.tile != currentTile)
        {
            currentTile = neededTile.tile;
            initialTilemap.SetTile(CellPosition, currentTile);
        }

        if (HP <= 0)
        {
            initialTilemap.SetTile(CellPosition, null);
            destroyedTilemap.SetTile(CellPosition, currentTile);
            OnWorkedOut?.Invoke(actionData);
        }
    }
}
