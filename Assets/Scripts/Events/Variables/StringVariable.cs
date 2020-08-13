using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StringUnityEvent : UnityEvent<string> { }


[CreateAssetMenu(fileName = "StringVariable", menuName = "Variables/StringVariable")]
public class StringVariable : ScriptableVariable<string> { }