using System;

[Serializable]
public class CharacterSkillInitData
{
    public CharacterSkill skill;
    public int value;
    public float learnability;

    public CharacterActionState State => (skill is CharacterActiveSkill) ? (skill as CharacterActiveSkill).state : null;
}
