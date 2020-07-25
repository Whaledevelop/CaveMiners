using UnityEngine;

[System.Serializable]
public struct CharacterActionData
{
    public CharacterStatesManager stateManager;
    public CharacterStateData stateData;
    public Vector2 startPosition;
    public Vector2 endPosition;

    public CharacterActionData(CharacterStatesManager stateManager, CharacterStateData stateData, Vector2 startPosition = default, Vector2 endPosition = default)
    {
        this.stateManager = stateManager;
        this.stateData = stateData;
        this.startPosition = startPosition;
        this.endPosition = endPosition;
    }

    public CharacterActionData(CharacterStatesManager stateManager, CharacterTaskPoint taskPoint)
    {
        this.stateManager = stateManager;
        this.stateData = taskPoint.stateData;
        this.startPosition = stateManager.transform.position;
        this.endPosition = taskPoint.CellPosition;
    }
}
