using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "MineCharacterState", menuName = "States/MineCharacterState")]
public class MineCharacterState : CharacterIterativeActionState
{
    [SerializeField] private CellPositionRequest basePositionRequest;

    [SerializeField] private TileBase baseTile;

    [SerializeField] private LayerMask baseLayer;

    [SerializeField] private ToolCode nextTaskDefaultTool;

    public override IEnumerator End()
    {
        yield return base.End();

        TaskStartData mineTask = actionData.taskManager.tasksHistory[actionData.taskManager.tasksHistory.Count - 1];
        ToolCode prevToolCode = toolsManager.defaultTool;

        toolsManager.defaultTool = nextTaskDefaultTool;

        basePositionRequest.MakeRequest(new ParamsObject(baseTile), out Vector2 basePosition);

        actionData.taskManager.nextTasks.Add(new TaskStartData(Utils.MaskToLayer(baseLayer), basePosition, () =>
        {
            toolsManager.defaultTool = prevToolCode;
            if (actionData.endExecutionCondition == EndExecutionCondition.IterationsCount)
            {
                actionData.taskManager.ExecuteTask(mineTask);
            }

        }));
    }
}