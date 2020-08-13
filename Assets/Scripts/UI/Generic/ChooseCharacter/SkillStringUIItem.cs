using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillStringUIItem : UIItem<CharacterActiveSkill>, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Text skillName;
    [SerializeField] private Text skillValue;

    public Action<CharacterActiveSkill> onPointerEnter;
    public Action<CharacterActiveSkill> onPointerExit;

    private CharacterActiveSkill skillData;

    public override void Init(CharacterActiveSkill setupData)
    {
        skillData = setupData;
        skillName.text = setupData.state.stateName;
        skillValue.text = setupData.Value.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(skillData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke(skillData);
    }
}
