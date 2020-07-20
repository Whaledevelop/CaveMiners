using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObjects/GameEvent")]
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

    public void Raise(ParamsObject eventParams)
    {
        foreach (GameEventListener listener in listeners)
            listener.OnEventRaised(eventParams);
    }
}
