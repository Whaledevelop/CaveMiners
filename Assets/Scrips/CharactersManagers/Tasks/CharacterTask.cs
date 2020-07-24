using System.Collections.Generic;
using System;

public class CharacterTask
{
    public List<StateAction> statesPoints = new List<StateAction>();
    public int currentStateIndex = -1;
    public bool isActive;

    public CharacterStatesManager statesManager;

    public Action OnEnd;

    public CharacterTask(List<StateAction> statesPoints, CharacterStatesManager statesManager)
    {
        this.statesPoints = statesPoints;
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
        if (currentStateIndex < statesPoints.Count)
        {
            statesManager.SetState(statesPoints[currentStateIndex].state, statesPoints[currentStateIndex].CellPosition);
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
