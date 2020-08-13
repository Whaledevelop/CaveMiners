using UnityEngine;
using UnityEngine.Events;

public class IntToStringVariableListener : IntVariableListener
{
    [SerializeField] private StringUnityEvent stringResponse;
    public override void OnEventRaised(int eventParams)
    {
        base.OnEventRaised(eventParams);
        stringResponse.Invoke(eventParams.ToString());
    }
}
