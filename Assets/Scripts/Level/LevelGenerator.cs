using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<GenerateRule> generateRules = new List<GenerateRule>();

    #region Инициализация персонажей

    [SerializeField] private CharacterInitialDataSet chosenCharacters;

    [SerializeField] private Character characterPrefab;

    [SerializeField] private Transform charactersParent;

    [SerializeField] private CellsPositionsRequest charactersInitPositionsRequest;

    [SerializeField] private CellPositionRequest cellCenterRequest;

    [SerializeField] private TileBase charactersInitTile;

    [SerializeField] private FogOfWar fogOfWar;

    [SerializeField] private CameraController cameraController;

    #endregion

    public void GenerateLevel(LevelSettings levelSettings)
    {
        GenerateLevelTiles(levelSettings);
        fogOfWar.Init(levelSettings);
        GenerateCharacters();
        cameraController.Init();
    }

    private void GenerateLevelTiles(LevelSettings levelSettings)
    {
        foreach (GenerateRule generateRule in generateRules)
        {
            generateRule.Init(levelSettings);
        }
        for (int x = levelSettings.xLevelSizeRange.from - 1; x < levelSettings.xLevelSizeRange.to + 1; x++)
        {
            for (int y = levelSettings.yLevelSizeRange.from - 1; y < levelSettings.yLevelSizeRange.to + 1; y++)
            {
                foreach (GenerateRule generateRule in generateRules)
                {
                    if (generateRule.HandlePosition(x, y))
                    {
                        if (generateRule.isSingleOnCell)
                            break;
                    }
                }
            }
        }
    }

    private void GenerateCharacters()
    {
        if (charactersInitPositionsRequest.MakeRequest(new ParamsObject(charactersInitTile), out List<Vector2> positionForChracters))
        {
            for (int i = 0; i < chosenCharacters.Items.Count; i++)
            {
                Character characterObject = Instantiate(characterPrefab, charactersParent);

                Vector2 position = i < positionForChracters.Count ? positionForChracters[i] : positionForChracters[0];

                cellCenterRequest.MakeRequest(new ParamsObject(position), out Vector2 cellCenter);

                characterObject.transform.position = cellCenter;

                characterObject.Init(chosenCharacters.Items[i]);

                fogOfWar.UpdateFogOfWar(cellCenter, characterObject.GetManager<CharacterSkillsManager>());
            }
        }
    }
}
