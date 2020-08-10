using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class CharacterTask
{
    public List<CharacterTaskPoint> taskPoints = new List<CharacterTaskPoint>();

    private CharacterToolsManager toolsManager;
    private Animator animator;
    private Rotator rotator;
    private CharacterSkillsManager skillsManager;
    private CharacterTasksManager taskManager;
    private CharacterActionHandler[] actionsHandlers;

    private int currentTaskPointIndex = 0;
    private CharacterTaskPoint CurrentTaskPoint => (currentTaskPointIndex < taskPoints.Count && currentTaskPointIndex >= 0 ) ? taskPoints[currentTaskPointIndex] : null;
    private CharacterActionState CurrentStateData => CurrentTaskPoint != null ? CurrentTaskPoint.stateData : null;
    private CharacterActionState activeState;

    public CharacterTask(List<CharacterTaskPoint> taskPoints, CharacterTasksManager taskManager, CharacterToolsManager toolsManager, 
        CharacterSkillsManager skillsManager, Animator animator, Rotator rotator, CharacterActionHandler[] actionsHandlers)
    {
        this.taskPoints = taskPoints;
        this.toolsManager = toolsManager;
        this.skillsManager = skillsManager;
        this.taskManager = taskManager;
        this.animator = animator;
        this.rotator = rotator;
        this.actionsHandlers = actionsHandlers;
    }

    public IEnumerator Execute()
    {
        while (currentTaskPointIndex < taskPoints.Count)
        {
            yield return ExecuteState(CurrentStateData, CurrentTaskPoint.CellPosition, -CurrentTaskPoint.AxisToNextCell);
            currentTaskPointIndex++;
        }
    }

    public IEnumerator ExecuteState(CharacterActionState state, Vector2 endPosition, Vector2 actionDirection)
    {
        CharacterAction actionData = new CharacterAction(taskManager, skillsManager, state, taskManager.transform.position, endPosition, actionDirection);

        CharacterActionHandler actionHandler = actionsHandlers.FirstOrDefault(handler => handler.HandledState == state);

        activeState = ScriptableObject.Instantiate(state);
        activeState.InitInstance(animator, toolsManager, rotator, actionData, actionHandler);

        yield return activeState.Execute();
    }

    public IEnumerator Cancel() 
    {
        yield return activeState.Cancel();
    }
}
