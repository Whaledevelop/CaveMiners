using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

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
        Response.Invoke();
    }
}

public abstract class GameEventListener<T> : MonoBehaviour
{
    public abstract GameEvent<T> Event { get; }
    public abstract UnityEvent<T> Response { get; }

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(T eventParam)
    {
        Response.Invoke(eventParam);
    }
}