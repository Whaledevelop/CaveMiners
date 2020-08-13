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

        yield return actionData.taskManager.ExecuteState(moveState, actionData.endPosition, actionData.actionDirection);
    }
}