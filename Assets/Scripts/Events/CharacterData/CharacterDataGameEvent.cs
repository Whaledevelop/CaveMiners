using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CharacterDataGameEvent", menuName = "Events/CharacterDataGameEvent")]
public class CharacterDataGameEvent : GameEvent<CharacterInitialData> { }

[System.Serializable]
public class CharacterDataUnityEvent : UnityEvent<CharacterInitialData> { }