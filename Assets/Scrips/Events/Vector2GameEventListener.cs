using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[System.Serializable]
public class Vector2UnitEvent : UnityEvent<Vector2> { }

[CreateAssetMenu(fileName = "Vector2GameEvent", menuName = "Events/Vector2GameEvent")]
public class Vector2GameEvent : GameEvent<Vector2> { }

public class Vector2GameEventListener : GameEventListener<Vector2>
{
    [SerializeField] private Vector2GameEvent vector2Event;
    [SerializeField] private Vector2UnitEvent vector2Response;

    public override GameEvent<Vector2> Event => vector2Event;

    public override UnityEvent<Vector2> Response => vector2Response;
}