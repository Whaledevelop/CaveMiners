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
    public CharacterActionState stateData;

    public Vector2 startPosition;
    public Vector2 endPosition;
    public Vector2 actionDirection;

    public Vector3Int endCellPosition;

    private CharacterSkill actionSkill;

    public CharacterSkill ActionSkill
    { 
        get
        {
            if (actionSkill == null)
                actionSkill = skillsManager.GetSkill(stateData.skillCode);

            return actionSkill;
        }

    }
    public float SkillValue => ActionSkill.Value;

    public CharacterAction(CharacterTasksManager taskManager, CharacterSkillsManager skillsManager, CharacterActionState stateData, Vector2 startPosition, Vector2 endPosition, Vector2 actionDirection)
    {
        this.taskManager = taskManager;
        this.skillsManager = skillsManager;
        this.stateData = stateData;
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.actionDirection = actionDirection;
    }

    public void LearnSkill()
    {
        ActionSkill.LearnSkill();
    }
}
