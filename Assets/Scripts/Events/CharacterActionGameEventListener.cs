using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[System.Serializable]
public class CharacterActionUnityEvent : UnityEvent<CharacterActionData> { }

[CreateAssetMenu(fileName = "CharacterActionGameEvent", menuName = "Events/CharacterAction")]
public class CharacterActionGameEvent : GameEvent<CharacterActionData> { }

public class CharacterActionGameEventListener : GameEventListener<CharacterActionData>
{
    [SerializeField] private CharacterActionGameEvent actionEvent;
    [SerializeField] private CharacterActionUnityEvent actionResponse;

    public override GameEvent<CharacterActionData> Event => actionEvent;

    public override UnityEvent<CharacterActionData> Response => actionResponse;
}
