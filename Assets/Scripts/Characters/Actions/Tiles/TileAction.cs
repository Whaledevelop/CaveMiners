using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileAction", menuName = "ScriptableObjects/TileAction")]
public class TileAction : ScriptableObject
{
    // Данные из эдитора
    [SerializeField] private CharacterActionState actionState;
    [SerializeField] private GenerativeTilemapsSet tilemapsSet;
    [SerializeField] private GenerativeTilemapCode initialTilemapCode;
    [SerializeField] private GenerativeTilemapCode destroyedTilemapCode;
    [SerializeField] private List<TileDependOnHP> tilesSet;
    [SerializeField] private float initialHP;

    // Данные экземпляра
    [NonSerialized] public CharacterAction actionData;
    public Action<TileAction> OnWorkedOut;
    private TileBase currentTile;
    private float HP;  
    private Vector3Int cellPosition;
    private GenerativeTilemap initialTilemap;
    private GenerativeTilemap destroyedTilemap;

    public CharacterActionState State => actionState;

    public void Init(CharacterAction actionData, Vector3Int cellPosition)
    {
        this.actionData = actionData;
        this.cellPosition = cellPosition;
        initialTilemap = tilemapsSet.FindByCode(initialTilemapCode);
        destroyedTilemap = tilemapsSet.FindByCode(destroyedTilemapCode);
        HP = initialHP;
    }

    public void Damage(float damage)
    {
        HP -= damage;

        //Debug.Log("Damage : " + damage + ", " + HP);
        TileDependOnHP neededTile = tilesSet.LastOrDefault(tileData => tileData.HP >= HP);
        if (neededTile != null && neededTile.tile != currentTile)
        {
            currentTile = neededTile.tile;
            initialTilemap.SetTile(cellPosition, currentTile);
        }

        if (HP <= 0)
        {
            initialTilemap.SetTile(cellPosition, null);
            destroyedTilemap.SetTile(cellPosition, currentTile);
            OnWorkedOut?.Invoke(this);
        }
    }

    public override string ToString()
    {
        return actionState + " - " + cellPosition + " - " + HP;
    }
}
