using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TunnelsController : MonoBehaviour
{
    [SerializeField] private Tile tunnelTile;
    [SerializeField] private Tilemap tilemap;

    public void OnDig(TileData tileData)
    {
        tilemap.SetTile(tileData.position, tunnelTile);
    }
}
