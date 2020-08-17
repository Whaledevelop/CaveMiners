using System;

/// <summary>
/// Активный скилл - это тот, который соответствует определенному состоянию (копание, разработка, передвижение)
/// </summary>
[Serializable]
public class CharacterActiveSkill : CharacterSkill
{
    public CharacterActionState state;
}
