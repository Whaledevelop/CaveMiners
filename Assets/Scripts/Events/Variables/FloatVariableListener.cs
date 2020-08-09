using UnityEngine;
using UnityEngine.Events;

public class FloatVariableListener : GameEventListener<float>
{
    [SerializeField] private FloatVariable variable;
    [SerializeField] private FloatUnityEvent actionResponse;

    public override GameEvent<float> Event => variable;

    public override UnityEvent<float> Response => actionResponse;

    private void Start()
    {
        Response.Invoke(variable.Value);
    }
}
