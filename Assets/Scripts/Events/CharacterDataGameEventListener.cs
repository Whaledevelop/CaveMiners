using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CharacterEvent", menuName = "Events/CharacterEvent")]
public class CharacterDataGameEvent : GameEvent<CharacterInitialData> { }

[System.Serializable]
public class CharacterDataUnityEvent : UnityEvent<CharacterInitialData> { }

public class CharacterDataGameEventListener : GameEventListener<CharacterInitialData>
{
    [SerializeField] private CharacterDataGameEvent gameEvent;

    [SerializeField] private CharacterDataUnityEvent response;
    public override GameEvent<CharacterInitialData> Event => gameEvent;

    public override UnityEvent<CharacterInitialData> Response => response;


}
