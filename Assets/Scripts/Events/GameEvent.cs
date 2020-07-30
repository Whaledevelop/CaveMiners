using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameEvent", menuName = "Events/NoParamsEvent")]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();
    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Raise()
    {
        foreach (GameEventListener listener in listeners)
            listener.OnEventRaised();
    }
}

public class GameEvent<T> : ScriptableObject
{
    private List<GameEventListener<T>> listeners = new List<GameEventListener<T>>();
    public void RegisterListener(GameEventListener<T> listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener<T> listener)
    {
        listeners.Remove(listener);
    }

    public virtual void Raise(T raiseParams)
    {
        foreach (GameEventListener<T> listener in listeners)
            listener.OnEventRaised(raiseParams);
    }
}

public class GameEvent<T1, T2> : ScriptableObject
{
    private List<GameEventListener<T1, T2>> listeners = new List<GameEventListener<T1, T2>>();
    public void RegisterListener(GameEventListener<T1, T2> listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener<T1, T2> listener)
    {
        listeners.Remove(listener);
    }

    public void Raise(T1 eventParam1, T2 eventParam2)
    {
        foreach (GameEventListener<T1, T2> listener in listeners)
            listener.OnEventRaised(eventParam1, eventParam2);
    }
}
