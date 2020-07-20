using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Vector2Event", menuName = "ScriptableObjects/Vector2Event")]
public class Vector2Event : GameEvent<Vector2> { }

[System.Serializable] public class Vector2UnityEvent : UnityEvent<Vector2> { }

public class Vector2EventListener : GameEventListener<Vector2>
{
    [SerializeField] private Vector2Event gameEvent;
    [SerializeField] private Vector2UnityEvent response;

    public override GameEvent<Vector2> Event => gameEvent;
    public override UnityEvent<Vector2> Response => response;
}
