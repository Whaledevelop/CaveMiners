using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class DigableGroundController : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Grid grid;

    public void OnDig(TileData tileData)
    {
        tilemap.SetTile(tileData.position, null);
    }
}
