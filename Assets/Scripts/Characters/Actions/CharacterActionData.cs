using System.Collections;
using UnityEngine;

public delegate IEnumerator NoParamsVoidDelegate();

[System.Serializable]
public struct CharacterActionData
{
    public CharacterTasksManager taskManager;
    public CharacterSkillsManager skillsManager;
    public CharacterStateData stateData;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public Vector2 actionDirection;
    public float stateSkill => skillsManager.GetStateSkill(stateData);

    public NoParamsVoidDelegate OnExecuteDelegate;

    public CharacterActionData(CharacterTasksManager taskManager, CharacterSkillsManager skillsManager, CharacterStateData stateData, Vector2 startPosition, Vector2 endPosition, Vector2 actionDirection, NoParamsVoidDelegate OnExecuteDelegate = null)
    {
        this.taskManager = taskManager;
        this.skillsManager = skillsManager;
        this.stateData = stateData;
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.actionDirection = actionDirection;
        this.OnExecuteDelegate = OnExecuteDelegate;
    }

    public void OnExecute()
    {
        if (OnExecuteDelegate != null)
        {
            IEnumerator executeEnumerator = OnExecuteDelegate.Invoke();
            if (executeEnumerator != default)
                taskManager.StartCoroutine(executeEnumerator);
        }            
    }
}
