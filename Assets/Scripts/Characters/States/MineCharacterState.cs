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

        // Определяем правый нижний участок базы
        Vector2 baseGates = basePositions.Aggregate(basePositions[0], (picked, next) =>
        {
            return next.x > picked.x || next.y < picked.y ? next : picked;
        });

        yield return actionData.taskManager.ExecuteTaskEnumerator(Utils.MaskToLayer(baseLayer), baseGates);

        toolsManager.defaultTool = prevToolCode;

        cellLayoutRequest.MakeRequest(new ParamsObject(actionData.endPosition), out LayerMask newTaskLayer);

        actionData.taskManager.ExecuteTask(newTaskLayer, actionData.endPosition);
    }
}