using System;
using System.Collections;
using UnityEngine;

public class DigActionHandler : CharacterActionHandler
{
    [SerializeField] private DigCharacterState digState;

    private bool isDigging;

    private CharacterAction actionData;

    public override CharacterActionState HandledState => digState;

    public override IEnumerator Execute(CharacterAction actionData)
    {
        isDigging = true;
        this.actionData = actionData;
        while (isDigging)
        {
            yield return new WaitForSeconds(digState.iterationsInterval);
            digState.iterationEvent.Raise(actionData);
            actionData.LearnSkill();
        }
        this.actionData = null;
    }

    public void OnTileWorkedOut(CharacterAction tileWorkedOutActionData)
    {
        if (actionData != null)
        {
            bool currentState = tileWorkedOutActionData.stateData == HandledState;
            bool currentTile = tileWorkedOutActionData.endPosition == actionData.endPosition;
            if (currentState && currentTile)
            {
                isDigging = false;                
            }
        }       
    }

    public override IEnumerator Cancel()
    {
        isDigging = false;
        yield break;
    }
}
