using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "MineCharacterState", menuName = "States/MineCharacterState")]
public class MineCharacterState : IterativeStateData
{
    [SerializeField] private CellPositionRequest basePositionRequest;

    [SerializeField] private TileBase baseTile;

    [SerializeField] private LayerMask baseLayer;

    [SerializeField] private ToolCode nextTaskDefaultTool;

    public override IEnumerator End(bool isNextStateTheSame, CharacterActionData actionData, Animator animator, CharacterToolsManager toolsManager, Rotator rotator)
    {
        yield return base.End(isNextStateTheSame, actionData, animator, toolsManager, rotator);

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