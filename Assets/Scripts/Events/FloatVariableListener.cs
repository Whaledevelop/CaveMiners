using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FloatUnityEvent : UnityEvent<float> { }


[CreateAssetMenu(fileName = "FloatVariable", menuName = "Variables/FloatVariable")]
public class FloatVariable : ScriptableVariable<float> 
{ 
    public void Plus(float plus)
    {
        Value += plus;
    }
}

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
