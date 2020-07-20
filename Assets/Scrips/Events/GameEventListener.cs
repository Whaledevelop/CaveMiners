using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventWithParams : UnityEvent<ParamsObject> { }

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent ResponseWithNoParams;
    public UnityEventWithParams ResponseWithParams;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        ResponseWithNoParams.Invoke();
        ResponseWithParams.Invoke(null);
    }

    public void OnEventRaised(ParamsObject eventParams)
    {
        ResponseWithNoParams.Invoke();
        ResponseWithParams.Invoke(eventParams);
    }

}