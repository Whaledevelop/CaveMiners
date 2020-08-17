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

    [SerializeField] private IntVariable moneyVariable;

    public CharacterActionGameEvent iterationEvent;

    public float iterationsInterval = 1;

    public int maxIterations;

    public override IEnumerator OnEnd()
    {
        yield return base.OnEnd();

        ToolCode prevToolCode = toolsManager.defaultTool;

        // Даем мешок с добычей
        toolsManager.defaultTool = nextTaskDefaultTool;

        // Определяем правый нижний участок базы - куда будем нести добычу
        basePositionRequest.MakeRequest(new ParamsObject(baseTile), out List<Vector2> basePositions);
        
        Vector2 baseGates = basePositions.Aggregate(basePositions[0], (picked, next) =>
        {
            return next.x > picked.x || next.y < picked.y ? next : picked;
        });

        CharacterTask returnToBaseTask = actionData.taskManager.ActivateTask(Utils.MaskToLayer(baseLayer), baseGates);
        // Ждем пока доставит добычу на базу
        yield return returnToBaseTask.Execute();

        // Добавляем деньги
        moneyVariable.Plus(maxIterations * actionData.SkillValue);

        // Убираем мешок, возвращаем предыдущий иструмент
        toolsManager.defaultTool = prevToolCode;

        cellLayoutRequest.MakeRequest(new ParamsObject(actionData.endPosition), out LayerMask newTaskLayer);

        // После возвращения на базу выполняем новое задание с той же точкой (если ресурсы еще не разработаны - 
        // доразработает, если уже - то дойдет)
        actionData.taskManager.ExecuteTask(newTaskLayer, actionData.endPosition);
    }
}