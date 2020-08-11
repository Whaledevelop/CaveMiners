using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "MineCharacterState", menuName = "States/MineCharacterState")]
public class MineCharacterState : CharacterActionState
{
    [SerializeField] private CellsPositionsRequest basePositionRequest;

    [SerializeField] private CellLayoutRequest cellLayoutRequest;

    [SerializeField] private TileBase baseTile;

    [SerializeField] private LayerMask baseLayer;

    [SerializeField] private ToolCode nextTaskDefaultTool;

    public override IEnumerator OnEnd()
    {
        yield return base.OnEnd();

        ToolCode prevToolCode = toolsManager.defaultTool;

        toolsManager.defaultTool = nextTaskDefaultTool;

        basePositionRequest.MakeRequest(new ParamsObject(baseTile), out List<Vector2> basePositions);

        Vector2 nearestBase = DefineNearestTo(actionData.taskManager.transform.position, basePositions); // В планах сделать несколько баз

        yield return actionData.taskManager.ExecuteTaskEnumerator(Utils.MaskToLayer(baseLayer), nearestBase);

        toolsManager.defaultTool = prevToolCode;

        cellLayoutRequest.MakeRequest(new ParamsObject(actionData.endPosition), out LayerMask newTaskLayer);

        actionData.taskManager.ExecuteTask(newTaskLayer, actionData.endPosition);
    }

    private Vector2 DefineNearestTo(Vector2 position, List<Vector2> amongPositions)
    {
        return amongPositions.Aggregate((nearest, next) => Vector2.Distance(next, position) < Vector2.Distance(nearest, position) ? next : nearest);
    }
}