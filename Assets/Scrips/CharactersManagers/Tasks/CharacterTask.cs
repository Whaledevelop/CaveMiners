using System.Collections.Generic;
using System;

public class CharacterTask
{
    public List<CharacterTaskPoint> taskPoints = new List<CharacterTaskPoint>();
    public int currentStateIndex = -1;
    public bool isActive;

    public CharacterStatesManager statesManager;

    public Action OnEnd;

    public CharacterTask(List<CharacterTaskPoint> taskPoints, CharacterStatesManager statesManager)
    {
        this.taskPoints = taskPoints;
        this.statesManager = statesManager;
    }


    public void Start()
    {
        SetNextState();
        statesManager.onEndState += SetNextState;
    }

    public void SetNextState()
    {
        currentStateIndex++;
        if (currentStateIndex < taskPoints.Count)
        {
            CharacterActionData actionData = new CharacterActionData(statesManager, taskPoints[currentStateIndex]);
            statesManager.SetState(actionData);
        }
        else
        {
            End();
        }

    }

    public void Cancel() 
    {
        statesManager.EndState();
    }

    public void End()
    {
        statesManager.onEndState -= SetNextState;
        OnEnd?.Invoke();
    }
}
