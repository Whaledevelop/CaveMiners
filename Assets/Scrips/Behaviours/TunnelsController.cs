using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TunnelsController : MonoBehaviour
{
    [SerializeField] private Tile tunnelTile;
    [SerializeField] private Tilemap tilemap;

    public void OnDig(Vector2 digPosition)
    {
        tilemap.SetTile(Vector3Int.FloorToInt(digPosition), tunnelTile);
    }
}
