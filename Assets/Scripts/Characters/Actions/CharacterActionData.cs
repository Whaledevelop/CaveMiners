using UnityEngine;

[System.Serializable]
public struct CharacterActionData
{
    public CharacterTasksManager taskManager;
    public CharacterStateData stateData;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public Vector2 actionDirection;
    public float stateSkill;

    public CharacterActionData(CharacterTasksManager taskManager, CharacterStateData stateData, Vector2 startPosition, Vector2 endPosition, Vector2 actionDirection, float stateSkill)
    {
        this.taskManager = taskManager;
        this.stateData = stateData;
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.actionDirection = actionDirection;
        this.stateSkill = stateSkill;
    }
}
