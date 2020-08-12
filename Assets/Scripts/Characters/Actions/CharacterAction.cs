using System.Collections;
using UnityEngine;

public enum EndExecutionCondition
{
    None,
    Executed,
    IterationsCount
}

[System.Serializable]
public class CharacterAction
{
    public CharacterTasksManager taskManager;
    public CharacterSkillsManager skillsManager;
    public CharacterState stateData;

    public Vector2 startPosition;
    public Vector2 endPosition;
    public Vector2 actionDirection;

    public Vector3Int endCellPosition;

    public float stateSkill => skillsManager.GetStateSkill(stateData);

    public CharacterAction(CharacterTasksManager taskManager, CharacterSkillsManager skillsManager, CharacterState stateData, Vector2 startPosition, Vector2 endPosition, Vector2 actionDirection)
    {
        this.taskManager = taskManager;
        this.skillsManager = skillsManager;
        this.stateData = stateData;
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.actionDirection = actionDirection;
    }
}
