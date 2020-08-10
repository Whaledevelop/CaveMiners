using System.Collections;
using UnityEngine;

public delegate IEnumerator NoParamsEnumeratorDelegate();

public enum EndExecutionCondition
{
    None,
    Executed,
    IterationsCount
}

[System.Serializable]
public struct CharacterActionData
{
    public CharacterTasksManager taskManager;
    public CharacterSkillsManager skillsManager;
    public CharacterState stateData;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public Vector2 actionDirection;
    public float stateSkill => skillsManager.GetStateSkill(stateData);

    public NoParamsEnumeratorDelegate OnExecuteDelegate;

    public EndExecutionCondition endExecutionCondition;

    public CharacterActionData(CharacterTasksManager taskManager, CharacterSkillsManager skillsManager, CharacterState stateData, Vector2 startPosition, Vector2 endPosition, Vector2 actionDirection, NoParamsEnumeratorDelegate OnExecuteDelegate = null)
    {
        this.taskManager = taskManager;
        this.skillsManager = skillsManager;
        this.stateData = stateData;
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.actionDirection = actionDirection;
        this.OnExecuteDelegate = OnExecuteDelegate;
        endExecutionCondition = EndExecutionCondition.None;
    }

    public void OnExecute(EndExecutionCondition endExecutionCondition)
    {
        this.endExecutionCondition = endExecutionCondition;
        if (OnExecuteDelegate != null)
        {
            IEnumerator executeEnumerator = OnExecuteDelegate.Invoke();
            if (executeEnumerator != default)
                taskManager.StartCoroutine(executeEnumerator);
        }        
    }
}
