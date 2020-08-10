using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterActionUnityEvent : UnityEvent<CharacterAction> { }

[CreateAssetMenu(fileName = "CharacterActionGameEvent", menuName = "Events/CharacterActionGameEvent")]
public class CharacterActionGameEvent : GameEvent<CharacterAction> { }