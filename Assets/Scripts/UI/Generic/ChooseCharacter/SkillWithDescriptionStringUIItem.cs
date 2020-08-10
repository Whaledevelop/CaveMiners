using UnityEngine;
using UnityEngine.UI;

public class SkillWithDescriptionStringUIItem : SkillStringUIItem
{
    [SerializeField] private Text descriptionString;

    public override void Init(CharacterStateSkillData setupData)
    {
        base.Init(setupData);
        //descriptionString.text = setupData.state.skillDescription;
    }
}
