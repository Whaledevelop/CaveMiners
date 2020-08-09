using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterActionUnityEvent : UnityEvent<CharacterActionData> { }

[CreateAssetMenu(fileName = "CharacterActionGameEvent", menuName = "Events/CharacterAction")]
public class CharacterActionGameEvent : GameEvent<CharacterActionData> { }