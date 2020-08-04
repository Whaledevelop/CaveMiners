using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterInitialData", menuName = "ScriptableObjects/CharacterInitialData")]
public class CharacterInitialData : ScriptableObject
{
    public new string name;
    [Multiline] public string description;

    public int salary;

    public string spriteName;

    public List<CharacterStateSkillData> initialSkillsData;
}