using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class CharacterTask
{
    public List<CharacterTaskPoint> taskPoints = new List<CharacterTaskPoint>();

    private CharacterToolsManager toolsManager;
    private Animator animator;
    private Rotator rotator;
    private CharacterSkillsManager skillsManager;
    private CharacterTasksManager taskManager;

    private int currentTaskPointIndex = -1;
    private CharacterTaskPoint CurrentTaskPoint => (currentTaskPointIndex < taskPoints.Count && currentTaskPointIndex >= 0 ) ? taskPoints[currentTaskPointIndex] : null;
    private CharacterStateData CurrentStateData => CurrentTaskPoint != null ? CurrentTaskPoint.stateData : null;

    private CharacterState activeState;

    public CharacterTask(List<CharacterTaskPoint> taskPoints, CharacterTasksManager taskManager, CharacterToolsManager toolsManager, 
        CharacterSkillsManager skillsManager, Animator animator, Rotator rotator)
    {
        this.taskPoints = taskPoints;
        this.toolsManager = toolsManager;
        this.skillsManager = skillsManager;
        this.taskManager = taskManager;
        this.animator = animator;
        this.rotator = rotator;
    }


    public IEnumerator ExecuteNextState()
    {
        CharacterStateData prevStateData = CurrentStateData;
        currentTaskPointIndex++;
        bool isCurrentStateTheSame = prevStateData != null && prevStateData == CurrentStateData;

        if (activeState != null)
        {
            yield return activeState.End(isCurrentStateTheSame);
        }
        if (currentTaskPointIndex < taskPoints.Count)
        {
            CharacterActionData actionData = new CharacterActionData(taskManager, skillsManager, CurrentStateData, taskManager.transform.position, 
                CurrentTaskPoint.CellPosition, -CurrentTaskPoint.AxisToNextCell, ExecuteNextState);
            activeState = new CharacterState(actionData, animator, toolsManager, rotator);
            yield return activeState.Execute(isCurrentStateTheSame);
        }
        else
        {
            End();
            yield break;
        }
    }

    public IEnumerator Cancel() 
    {
        yield return activeState.End();
    }

    public void End()
    {
        
    }
}
