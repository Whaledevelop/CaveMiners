using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Данные конкретного персонажа
/// </summary>
[CreateAssetMenu(fileName = "CharacterInitialData", menuName = "ScriptableObjects/CharacterInitialData")]
public class CharacterInitialData : ScriptableObject
{
    public new string name;
    [Multiline] public string description;

    public int salary;

    public string spriteName;

    public List<CharacterActiveSkill> activeSkills = new List<CharacterActiveSkill>();
    public List<CharacterSkill> passiveSkills= new List<CharacterSkill>();
}