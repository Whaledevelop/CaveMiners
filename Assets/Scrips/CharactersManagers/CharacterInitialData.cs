﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterInitialData", menuName = "ScriptableObjects/CharacterInitialData")]
public class CharacterInitialData : ScriptableObject
{
    public new string name;
    [Multiline] public string description;

    public List<CharacterStateSkillData> initialSkillsData;
}