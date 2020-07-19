using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DigEvent", menuName = "ScriptableObjects/DigEvent")]
public class DigEvent : GameEvent<TileData> { }

[System.Serializable]
public class TileUnityEvent : UnityEvent<TileData> { }

public class DigEventListener : GameEventListener<TileData>
{
    [SerializeField] private DigEvent digEvent;
    [SerializeField] private TileUnityEvent digResponse;

    public override GameEvent<TileData> Event => digEvent;

    public override UnityEvent<TileData> Response => digResponse;
}
