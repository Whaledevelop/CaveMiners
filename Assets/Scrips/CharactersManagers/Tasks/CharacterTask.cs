using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterTask
{
    public List<CharacterTaskPoint> taskPoints = new List<CharacterTaskPoint>();

    private CharacterToolsManager toolsManager;
    private Animator animator;
    private CharacterSkillsManager skillsManager;
    private CharacterTasksManager taskManager;

    private int currentTaskPointIndex = -1;
    private CharacterTaskPoint CurrentTaskPoint => (currentTaskPointIndex < taskPoints.Count && currentTaskPointIndex >= 0 ) ? taskPoints[currentTaskPointIndex] : null;
    private CharacterStateData CurrentStateData => CurrentTaskPoint != null ? CurrentTaskPoint.stateData : null;

    private CharacterState activeState;

    public CharacterTask(List<CharacterTaskPoint> taskPoints, CharacterTasksManager taskManager, CharacterToolsManager toolsManager, CharacterSkillsManager skillsManager, Animator animator)
    {
        this.taskPoints = taskPoints;
        this.toolsManager = toolsManager;
        this.skillsManager = skillsManager;
        this.taskManager = taskManager;
        this.animator = animator;
    }


    public void Start()
    {
        SetNextState();
    }

    public void SetNextState()
    {
        CharacterStateData prevStateData = CurrentStateData;
        currentTaskPointIndex++;
        bool isCurrentStateTheSame = prevStateData != null && prevStateData == CurrentStateData;
        if (activeState != null)
        {
            activeState.End(isCurrentStateTheSame);
        }
        if (currentTaskPointIndex < taskPoints.Count)
        {
            CharacterActionData actionData = new CharacterActionData(taskManager, CurrentStateData, taskManager.transform.position, CurrentTaskPoint.CellPosition);
            activeState = new CharacterState(actionData, skillsManager.GetStateSkill(CurrentStateData), animator, toolsManager);
            activeState.Start(isCurrentStateTheSame);
        }
        else
        {
            End();
        }
    }

    public void Cancel() 
    {
        //statesManager.EndState();
    }

    public void End()
    {
        //OnEnd?.Invoke();
    }
}
