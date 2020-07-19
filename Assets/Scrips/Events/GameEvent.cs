using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObjects/GameEvent")]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> listeners = new List<GameEventListener>();
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
    public List<GameEventListener<T>> listeners = new List<GameEventListener<T>>();
    public void RegisterListener(GameEventListener<T> listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener<T> listener)
    {
        listeners.Remove(listener);
    }

    public void Raise(T raiseParam)
    {
        foreach (GameEventListener<T> listener in listeners)
            listener.OnEventRaised(raiseParam);
    }
}
