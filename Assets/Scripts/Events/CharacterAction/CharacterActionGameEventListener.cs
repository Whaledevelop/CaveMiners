using UnityEngine;
using UnityEngine.Events;

public class CharacterActionGameEventListener : GameEventListener<CharacterActionData>
{
    [SerializeField] private CharacterActionGameEvent actionEvent;
    [SerializeField] private CharacterActionUnityEvent actionResponse;

    public override GameEvent<CharacterActionData> Event => actionEvent;

    public override UnityEvent<CharacterActionData> Response => actionResponse;
}
