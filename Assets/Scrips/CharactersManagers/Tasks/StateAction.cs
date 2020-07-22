using UnityEngine;
public class StateAction
{
    public CharacterStateData state;
    public Vector2 fromCellPosition;
    public Vector2 actionAxis;

    public string Name => state.stateName;
    public int Priority => state.actionPriority;
    public Vector2 ToCellPosition => fromCellPosition + actionAxis;

    public StateAction(CharacterStateData state, Vector2 fromCellPosition, Vector2 actionAxis)
    {
        this.state = state;
        this.fromCellPosition = fromCellPosition;
        this.actionAxis = actionAxis;
    }

    public override string ToString()
    {
        return Name + " - " + ToCellPosition + " - " + Priority;
    }
}