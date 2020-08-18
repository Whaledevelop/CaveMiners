using UnityEngine;
using UnityEngine.Events;
public class CharacterDataGameEventListener : GameEventListener<CharacterInitialData>
{
    [SerializeField] private CharacterDataGameEvent gameEvent;

    [SerializeField] private CharacterDataUnityEvent response;

    public override GameEvent<CharacterInitialData> Event => gameEvent;

    public override UnityEvent<CharacterInitialData> Response => response;
}
