using UnityEngine;
using UnityEngine.UI;

public class SkillWithDescriptionStringUIItem : SkillStringUIItem
{
    [SerializeField] private Text descriptionString;

    public override void Init(CharacterActiveSkill setupData)
    {
        base.Init(setupData);
        //descriptionString.text = setupData.state.skillDescription;
    }
}
