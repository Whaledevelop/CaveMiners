﻿using UnityEngine;
using System.Collections;

public class MineActionHandler : CharacterActionHandler
{
    [SerializeField] private MineCharacterState mineState;

    [SerializeField] private CharacterActionGameEvent iterationEvent;

    [SerializeField] private float iterationsInterval = 1;

    [SerializeField] private int maxIterations;

    private bool isMining;

    private int iterationCount;

    private CharacterAction actionData;

    public override CharacterActionState HandledState => mineState;

    public override IEnumerator Execute(CharacterAction actionData)
    {
        isMining = true;
        this.actionData = actionData;
        while (isMining && iterationCount < maxIterations)
        {
            yield return new WaitForSeconds(iterationsInterval);
            iterationEvent.Raise(actionData);
            iterationCount++;
        }
        EndIterations();
    }

    public void OnTileWorkedOut(CharacterAction tileWorkedOutActionData)
    {
        if (tileWorkedOutActionData.stateData == HandledState && tileWorkedOutActionData.endPosition == actionData.endPosition)
        {
            EndIterations();
        }
    }

    public override IEnumerator Cancel()
    {
        EndIterations();
        yield break;
    }

    private void EndIterations()
    {
        isMining = false;
        iterationCount = 0;
    }
}