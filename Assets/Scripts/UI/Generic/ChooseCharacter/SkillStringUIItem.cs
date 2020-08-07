using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillStringUIItem : UIItem<CharacterStateSkillData>, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Text skillName;
    [SerializeField] private Text skillValue;

    public Action<CharacterStateSkillData> onPointerEnter;
    public Action<CharacterStateSkillData> onPointerExit;

    CharacterStateSkillData skillData;

    public override void Init(CharacterStateSkillData setupData)
    {
        skillData = setupData;
        skillName.text = setupData.state.stateName;
        skillValue.text = setupData.value.ToString();
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
