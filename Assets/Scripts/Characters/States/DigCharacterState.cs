using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "DigCharacterState", menuName = "States/DigCharacterState")]
public class DigCharacterState : CharacterIterativeActionState
{
    [SerializeField] private CharacterActionState moveToPointState;

    [NonSerialized] private bool isMovedToPoint;

    [NonSerialized] private CharacterActionState instantiatedMoveToPointState;

    public override IEnumerator End()
    {
        isMovedToPoint = false;
        yield return base.End();

        CharacterActionData moveToDiggedPointAction = new CharacterActionData(actionData.taskManager, actionData.skillsManager, moveToPointState,
            actionData.taskManager.transform.position, actionData.endPosition, actionData.actionDirection, null);

        moveToDiggedPointAction.OnExecuteDelegate = OnMoveToPointEnumerator;

        instantiatedMoveToPointState = Instantiate(moveToPointState);
        instantiatedMoveToPointState.InitInstance(animator, toolsManager, rotator, moveToDiggedPointAction);
        yield return instantiatedMoveToPointState.Start();
        yield return new WaitUntil(() => isMovedToPoint);
    }

    public IEnumerator OnMoveToPointEnumerator()
    {
        yield return instantiatedMoveToPointState.End();
        isMovedToPoint = true;
    }
}