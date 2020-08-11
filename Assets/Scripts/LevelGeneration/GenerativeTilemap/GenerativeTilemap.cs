using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerativeTilemap : MonoBehaviour
{
    public GenerativeTilemapCode tilemapData;
    [SerializeField] private GenerativeTilemapsSet set;
    [SerializeField] private Tilemap tilemap;

    public void SetTile(Vector3Int position, TileBase tile)
    {
        tilemap.SetTile(position, tile);
    }

    public void OnEnable()
    {
        set.Add(this);
    }

    public void OnDisable()
    {
        set.Remove(this);
    }
}
