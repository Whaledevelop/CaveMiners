using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour
{
    [SerializeField] private Tilemap fogOfWarTilemap;
    [SerializeField] private TileBase fogOfWarTile;
    [SerializeField] private Grid grid;
    //[SerializeField] private CharacterState observeFogOfWarState;

    public void Init(LevelSettings levelSettings)
    {
        for(int x = levelSettings.xLevelSizeRange.from - 2; x < levelSettings.xLevelSizeRange.to + 2; x++)
        {
            for (int y = levelSettings.yLevelSizeRange.from - 2; y < levelSettings.yLevelSizeRange.to + 2; y++)
            {
                fogOfWarTilemap.SetTile(new Vector3Int(x, y, (int)fogOfWarTilemap.transform.position.z), fogOfWarTile);
            }
        }
    }

    public void OnActionReducingFogOfWar(CharacterAction action)
    {
        //float skill = action.skillsManager.GetStateSkill(observeFogOfWarState);
        UpdateFogOfWar(action.endPosition, 1);
    }

    public void UpdateFogOfWar(Vector2 reducerPosition, int reduceCellsRadius)
    {
        Vector3Int reducerCellPosition = grid.WorldToCell(reducerPosition);
        UpdateFogOfWar(reducerCellPosition, reduceCellsRadius);
    }

    public void UpdateFogOfWar(Vector3Int reducerPosition, int reduceCellsRadius)
    {
        for(int x = reducerPosition.x - reduceCellsRadius; x <= reducerPosition.x + reduceCellsRadius; x++)
        {
            for(int y = reducerPosition.y - reduceCellsRadius; y <= reducerPosition.y + reduceCellsRadius; y++)
            {
                Vector3Int localPlace = (new Vector3Int(x, y, (int)fogOfWarTilemap.transform.position.z));

                if (fogOfWarTilemap.GetTile(localPlace) == fogOfWarTile)
                {
                    fogOfWarTilemap.SetTile(localPlace, null);
                }
            }
        }
    }
}
