using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[System.Serializable]
public class CellActionUnitEvent : UnityEvent<Vector2, Vector2> { }

[CreateAssetMenu(fileName = "CellActionGameEvent", menuName = "Events/CellAction")]
public class CellActionGameEvent : GameEvent<Vector2, Vector2> { }

public class CellActionGameEventListener : GameEventListener<Vector2, Vector2>
{
    [SerializeField] private CellActionGameEvent cellActionEvent;
    [SerializeField] private CellActionUnitEvent cellActionResponse;

    public override GameEvent<Vector2, Vector2> Event => cellActionEvent;

    public override UnityEvent<Vector2, Vector2> Response => cellActionResponse;
}