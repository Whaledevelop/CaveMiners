using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class DigableGroundController : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Grid grid;

    public void OnDig(Vector2 diggerPosition, Vector2 digPosition)
    {
        tilemap.SetTile(Vector3Int.FloorToInt(digPosition), null);
    }
}
