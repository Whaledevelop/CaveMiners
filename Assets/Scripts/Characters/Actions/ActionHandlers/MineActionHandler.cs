using UnityEngine;
using System.Collections;

public class MineActionHandler : CharacterActionHandler
{
    [SerializeField] private MineCharacterState mineState;

    private bool isMining;

    private int iterationCount;

    private CharacterAction actionData;

    public override CharacterActionState HandledState => mineState;

    public override IEnumerator Execute(CharacterAction actionData)
    {
        isMining = true;
        this.actionData = actionData;
        while (isMining && iterationCount < mineState.maxIterations)
        {
            yield return new WaitForSeconds(mineState.iterationsInterval);
            mineState.iterationEvent.Raise(actionData);
            actionData.LearnSkill();
            iterationCount++;
        }
        EndIterations();
    }

    public void OnTileWorkedOut(CharacterAction tileWorkedOutActionData)
    {
        if (actionData != null)
        {
            if (tileWorkedOutActionData.stateData == HandledState && tileWorkedOutActionData.endPosition == actionData.endPosition)
            {
                EndIterations();
            }
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
