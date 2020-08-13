using UnityEngine;
using UnityEngine.Events;

public class IntVariableListener : GameEventListener<int>
{
    [SerializeField] private IntVariable variable;
    [SerializeField] private IntUnityEvent actionResponse;

    public override GameEvent<int> Event => variable;

    public override UnityEvent<int> Response => actionResponse;

    private void Start()
    {
        OnEventRaised(variable.Value);
    }
}
