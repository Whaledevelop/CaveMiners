using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "DigCharacterState", menuName = "States/DigCharacterState")]
public class DigCharacterState : CharacterActionState
{
    [SerializeField] private CharacterActionState moveState;

    public CharacterActionGameEvent iterationEvent;

    public float iterationsInterval = 1;

    public override IEnumerator OnEnd()
    {
        yield return base.OnEnd();
        // После выполнения копания выполняем переход к раскопанной клетке
        CharacterActionState activatedState = actionData.taskManager.ActivateState(moveState, actionData.endPosition, actionData.actionDirection);
        yield return activatedState.Execute();
    }
}