using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "CharacterInitialData", menuName = "ScriptableObjects/CharacterInitialData")]
public class CharacterInitialData : ScriptableObject
{
    public new string name;
    [Multiline] public string description;
    public int initialDigSkill;
}