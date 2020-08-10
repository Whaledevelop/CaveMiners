using UnityEngine;
using UnityEngine.Events;

public class CharacterActionGameEventListener : GameEventListener<CharacterAction>
{
    [SerializeField] private CharacterActionGameEvent actionEvent;
    [SerializeField] private CharacterActionUnityEvent actionResponse;

    public override GameEvent<CharacterAction> Event => actionEvent;

    public override UnityEvent<CharacterAction> Response => actionResponse;
}
