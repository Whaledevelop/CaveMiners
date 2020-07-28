using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DigHandler : MonoBehaviour, IIteractiveActionHandler
{
    [SerializeField] private List<Tilemap> diggableTilemaps = new List<Tilemap>();

    [SerializeField] private Grid grid;

    [SerializeField] private List<TileData> tilesSet = new List<TileData>();

    [SerializeField] private int initialHP;

    private List<DiggingTile> diggingTiles = new List<DiggingTile>();

    public void OnStartAction(CharacterActionData actionData)
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
