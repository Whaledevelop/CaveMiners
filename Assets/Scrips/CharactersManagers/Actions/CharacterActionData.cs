using UnityEngine;

[System.Serializable]
public struct CharacterActionData
{
    public CharacterTasksManager taskManager;
    public CharacterStateData stateData;
    public Vector2 startPosition;
    public Vector2 endPosition;

    public CharacterActionData(CharacterTasksManager taskManager, CharacterStateData stateData, Vector2 startPosition = default, Vector2 endPosition = default)
    {
        this.taskManager = taskManager;
        this.stateData = stateData;
        this.startPosition = startPosition;
        this.endPosition = endPosition;
    }
}
