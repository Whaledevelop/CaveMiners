using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Скрипт генерирующий и обновляющий "туман войны". Сделано при помощи отдельного tilemap, который рисует тайлы тумана
/// поверх всех других тайлов и при развеивании тумана удаляет тайлы.
/// </summary>
public class FogOfWar : MonoBehaviour
{
    [SerializeField] private Tilemap fogOfWarTilemap;
    [SerializeField] private TileBase fogOfWarTile;
    [SerializeField] private Grid grid;
    [SerializeField] private CharacterSkill.Code observeFogOfWarSkillCode;

    public void Init(LevelSettings levelSettings)
    {
        // Пока костыльное решение, нужно сделать генерацию в зависимости от максимально возможного обзора
        for(int x = levelSettings.XLevelSizeRange.from * 5; x < levelSettings.XLevelSizeRange.to * 5; x++)
        {
            for (int y = levelSettings.YLevelSizeRange.from * 5; y < levelSettings.YLevelSizeRange.to * 5; y++)
            {
                fogOfWarTilemap.SetTile(new Vector3Int(x, y, (int)fogOfWarTilemap.transform.position.z), fogOfWarTile);
            }
        }
    }

    /// <summary>
    /// Обновить туман войны при каком-то действии персонажа
    /// </summary>
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
        // Проходим по всем позициям в радиусе от точки развеивателя тумана. Радиус базируется на соответствующем таланте персонажа
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
