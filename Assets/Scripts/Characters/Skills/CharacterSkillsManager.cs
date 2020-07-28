using System.Collections.Generic;
using UnityEngine;

public class CharacterSkillsManager : MonoBehaviour
{
    [SerializeField] private CharacterInitialData initialData;

    private List<CharacterStateSkillData> skillsData = new List<CharacterStateSkillData>();

    private void Start()
    {
        skillsData = initialData.initialSkillsData; 
    }

    public float GetStateSkill(CharacterStateData state)
    {
        return skillsData.Find(skill => skill.state == state).value;
    }

    public void UpdateSkill(CharacterStateData state)
    {
        for(int i = 0; i < skillsData.Count; i++)
        {
            if (skillsData[i].state == state)
            {
                skillsData[i].value += skillsData[i].learnability;
            }
        }
    }
}
