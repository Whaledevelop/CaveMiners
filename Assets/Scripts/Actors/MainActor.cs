using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class MainActor : MonoBehaviour
{
    [SerializeField] private bool isGameEndable = true;
    [SerializeField] private GameEvent gameOverEvent;

    [SerializeField] private CharacterInitialDataSet chosenCharacters;

    [SerializeField] private CharacterManager characterPrefab;

    [SerializeField] private Transform charactersParent;

    [SerializeField] private LevelGenerator levelGenerator;

    [SerializeField] private CameraController cameraController;

    [SerializeField] private CellsPositionsRequest charactersInitPositionsRequest;

    [SerializeField] private CellPositionRequest cellCenterRequest;

    [SerializeField] private TileBase charactersInitTile;

    [SerializeField] private FogOfWar fogOfWar;

    [SerializeField] private LevelSettings levelSettings;

    public void Start()
    {
        levelGenerator.Generate(levelSettings);

        fogOfWar.Init(levelSettings);

        if (charactersInitPositionsRequest.MakeRequest(new ParamsObject(charactersInitTile), out List<Vector2> positionForChracters))
        {
            for (int i = 0; i < chosenCharacters.Items.Count; i++)
            {
                CharacterManager characterObject = Instantiate(characterPrefab, charactersParent);

                Vector2 position = i < positionForChracters.Count ? positionForChracters[i] : positionForChracters[0];

                cellCenterRequest.MakeRequest(new ParamsObject(position), out Vector2 cellCenter);

                characterObject.transform.position = cellCenter;

                characterObject.Init(chosenCharacters.Items[i]);

                fogOfWar.UpdateFogOfWar(cellCenter, 1);
            }
        }
        cameraController.Setup();
    }

    public void OnChangeMoney(float money)
    {
        if (money <= 0 && isGameEndable)
        {
            gameOverEvent.Raise();
        }            
    }

    public void OnBase()
    {
        //Debug.Log("base");
    }
}
