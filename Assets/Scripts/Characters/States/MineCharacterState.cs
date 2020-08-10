using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "MineCharacterState", menuName = "States/MineCharacterState")]
public class MineCharacterState : CharacterActionState
{
    [SerializeField] private CellPositionRequest basePositionRequest;

    [SerializeField] private CellLayoutRequest cellLayoutRequest;

    [SerializeField] private TileBase baseTile;

    [SerializeField] private LayerMask baseLayer;

    [SerializeField] private ToolCode nextTaskDefaultTool;

    public override IEnumerator OnEnd()
    {
        yield return base.OnEnd();

        ToolCode prevToolCode = toolsManager.defaultTool;

        toolsManager.defaultTool = nextTaskDefaultTool;

        basePositionRequest.MakeRequest(new ParamsObject(baseTile), out Vector2 basePosition);

        yield return actionData.taskManager.ExecuteTaskEnumerator(Utils.MaskToLayer(baseLayer), basePosition);

        toolsManager.defaultTool = prevToolCode;

        cellLayoutRequest.MakeRequest(new ParamsObject(actionData.endPosition), out LayerMask newTaskLayer);

        actionData.taskManager.ExecuteTask(newTaskLayer, actionData.endPosition);
    }
}