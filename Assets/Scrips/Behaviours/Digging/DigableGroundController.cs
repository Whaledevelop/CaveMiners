using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class DigableGroundController : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Grid grid;

    public void OnDig(ParamsObject digParams)
    {
        Debug.Log(digParams);
        Vector3Int digPosition = (Vector3Int)digParams.paramsArray[0];
        tilemap.SetTile(digPosition, null);
    }
}
