using System;
using System.Collections.Generic;
using UnityEngine;


public class CharacterSkillsManager : MonoBehaviour
{
    private List<CharacterActiveSkill> activeSkills = new List<CharacterActiveSkill>();
    private List<CharacterSkill> passiveSkills = new List<CharacterSkill>();

    public void Init(CharacterInitialData initialData)
    {
        foreach(CharacterSkill skill in initialData.passiveSkills)
        {
            passiveSkills.Add(skill);
        }
        foreach (CharacterActiveSkill skill in initialData.activeSkills)
        {
            activeSkills.Add(skill);
        }
    }

    public CharacterSkill GetSkill(CharacterSkill.Code code)
    {
        CharacterSkill skill = activeSkills.Find(s => s.code == code);
        if (skill == null)
            return passiveSkills.Find(s => s.code == code);
        else
            return skill;
    }
}
