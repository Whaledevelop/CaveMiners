using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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

[Serializable]
public class TileData
{
    public float HP;
    public TileBase tile;
}

public class DigHandler : CharacterActionGameEventListener
{
    [SerializeField] private List<Tilemap> diggableTilemaps = new List<Tilemap>();

    [SerializeField] private Grid grid;

    [SerializeField] private List<TileData> tilesSet = new List<TileData>();

    [SerializeField] private int initialHP;

    private List<DiggingTile> diggingTiles = new List<DiggingTile>();

    public override void OnEventRaised(CharacterActionData actionData)
    {
        Vector3Int digCellPositionInt = grid.WorldToCell(Vector3Int.FloorToInt(actionData.endPosition));

        foreach (Tilemap digableTilemap in diggableTilemaps)
        {
            TileBase digTile = digableTilemap.GetTile(digCellPositionInt);
            if (digTile != null)
            {
                DiggingTile diggingTile = new DiggingTile(tilesSet, digTile, digableTilemap, digCellPositionInt, actionData, initialHP);
                diggingTiles.Add(diggingTile);
                break;
            }
        }
    }

    public void OnIteration(CharacterActionData actionData)
    {
        DiggingTile digTile = diggingTiles.Find(diggingTile => diggingTile.activeActions.Contains(actionData));
        if (digTile != null)
        {
            digTile.Dig(actionData);
        }
    }
}

//[CustomEditor(typeof(MoveHandler)), CanEditMultipleObjects]
//public class MoveHandlerEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        EditorGUILayout.FloatField("CurrentSpeed", (target as MoveHandler).speed);
//    }
//}
