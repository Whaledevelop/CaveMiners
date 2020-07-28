using UnityEngine;
using UnityEditor;
using System.Collections;

[CreateAssetMenu(fileName = "DigCharacterState", menuName = "States/DigCharacterState")]
public class DigCharacterState : IterativeStateData
{
    [SerializeField] private CharacterStateData moveToPointState;

    private bool isMovedToPoint;

    public override IEnumerator End(bool isNextStateTheSame, CharacterActionData actionData, Animator animator, CharacterToolsManager toolsManager, Rotator rotator)
    {
        yield return base.End(isNextStateTheSame, actionData, animator, toolsManager, rotator);

        CharacterActionData moveToDiggedPointAction = new CharacterActionData(actionData.taskManager, actionData.skillsManager, moveToPointState, 
            actionData.taskManager.transform.position, actionData.endPosition, actionData.actionDirection, null);

        moveToDiggedPointAction.OnExecuteDelegate = () =>
        {
            OnMoveToPoint(moveToDiggedPointAction, animator, toolsManager, rotator);
            return default;
        };
        yield return moveToPointState.Execute(false, moveToDiggedPointAction, animator, toolsManager, rotator);
        yield return new WaitUntil(() => isMovedToPoint);
    }
    
    public void OnMoveToPoint(CharacterActionData actionData, Animator animator, CharacterToolsManager toolsManager, Rotator rotator)
    {
        Debug.Log("On move to point");
        isMovedToPoint = true;

        actionData.taskManager.StartCoroutine(moveToPointState.End(false, actionData, animator, toolsManager, rotator));
    }
}