using System;
using System.Collections;
using UnityEngine;

public class DigActionHandler : CharacterActionHandler
{
    [SerializeField] private DigCharacterState digState;

    [SerializeField] private CharacterActionGameEvent iterationEvent;

    [SerializeField] private float iterationsInterval = 1;

    private bool isDigging;

    private CharacterAction actionData;

    public override CharacterActionState HandledState => digState;

    public override IEnumerator Execute(CharacterAction actionData)
    {
        isDigging = true;
        this.actionData = actionData;
        while (isDigging)
        {
            yield return new WaitForSeconds(iterationsInterval);
            iterationEvent.Raise(actionData);
        }
    }

    public void OnTileWorkedOut(CharacterAction tileWorkedOutActionData)
    {
        if (tileWorkedOutActionData.stateData == HandledState && tileWorkedOutActionData.endPosition == actionData.endPosition)
        {
            isDigging = false;
        }        
    }

    public override IEnumerator Cancel()
    {
        isDigging = false;
        yield break;
    }
}
