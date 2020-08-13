using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour
{
    [SerializeField] private Tilemap fogOfWarTilemap;
    [SerializeField] private TileBase fogOfWarTile;
    [SerializeField] private Grid grid;
    [SerializeField] private CharacterSkill.Code observeFogOfWarSkillCode;

    public void Init(LevelSettings levelSettings)
    {
        for(int x = levelSettings.XLevelSizeRange.from * 5; x < levelSettings.XLevelSizeRange.to * 5; x++)
        {
            for (int y = levelSettings.YLevelSizeRange.from * 5; y < levelSettings.YLevelSizeRange.to * 5; y++)
            {
                fogOfWarTilemap.SetTile(new Vector3Int(x, y, (int)fogOfWarTilemap.transform.position.z), fogOfWarTile);
            }
        }
    }

    public void OnActionReducingFogOfWar(CharacterAction action)
    {
        UpdateFogOfWar(action.endPosition, action.skillsManager);
    }

    public void UpdateFogOfWar(Vector2 reducerPosition, CharacterSkillsManager skillsManager)
    {
        UpdateFogOfWar(grid.WorldToCell(reducerPosition), skillsManager.GetSkill(observeFogOfWarSkillCode).Value);
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
