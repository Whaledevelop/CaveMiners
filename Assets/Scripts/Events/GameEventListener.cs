using UnityEngine;
using System.Collections;
using UnityEngine.Events;

/// <summary>
/// Слушатель GameEvent события
/// </summary>
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

    public virtual void OnEnable()
    {
        Event.RegisterListener(this);
    }

    public virtual void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public virtual void OnEventRaised(T eventParams)
    {
        Response.Invoke(eventParams);
    }
}

public abstract class GameEventListener<T1, T2> : MonoBehaviour
{
    public abstract GameEvent<T1, T2> Event { get; }
    public abstract UnityEvent<T1, T2> Response { get; }

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(T1 eventParam1, T2 eventParam2)
    {
        Response.Invoke(eventParam1, eventParam2);
    }
}
