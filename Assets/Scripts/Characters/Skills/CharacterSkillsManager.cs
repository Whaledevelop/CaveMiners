using System;
using System.Collections.Generic;
using UnityEngine;


public class CharacterSkillsManager : CharacterManager
{
    private List<CharacterActiveSkill> activeSkills = new List<CharacterActiveSkill>();
    private List<CharacterSkill> passiveSkills = new List<CharacterSkill>();

    public override void Init(Character character)
    {
        base.Init(character);
        foreach(CharacterSkill skill in characterData.passiveSkills)
        {
            passiveSkills.Add(skill);
        }
        foreach (CharacterActiveSkill skill in characterData.activeSkills)
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
