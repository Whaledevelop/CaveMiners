using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[System.Serializable]
public class StateActionUnitEvent : UnityEvent<StateAction> { }

[CreateAssetMenu(fileName = "StateActionGameEvent", menuName = "Events/StateAction")]
public class StateActionGameEvent : GameEvent<StateAction> { }

public class StateActionGameEventListener : GameEventListener<StateAction>
{
    [SerializeField] private StateActionGameEvent stateActionEvent;
    [SerializeField] private StateActionUnitEvent stateActionResponse;

    public override GameEvent<StateAction> Event => stateActionEvent;

    public override UnityEvent<StateAction> Response => stateActionResponse;
}
