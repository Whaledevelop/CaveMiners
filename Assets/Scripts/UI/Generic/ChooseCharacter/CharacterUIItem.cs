using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class CharacterUIItem : UIItem<CharacterInitialData>
{
    [SerializeField] private Text nameText;

    [SerializeField] private SkillStringUIItem skillsStringsPrefab;

    [SerializeField] private RectTransform infoPanel;

    [SerializeField] private RawImage previewImage;

    [SerializeField] private CharacterPreviewUIItemSet previewUIItemSet;

    [HideInInspector] public CharacterInitialData character;

    protected CharacterPreview characterPreview;

    private List<SkillStringUIItem> skillsStrings = new List<SkillStringUIItem>();

    public override void Init(CharacterInitialData setupData)
    {
        character = setupData;
        nameText.text = setupData.name;

        StartCoroutine(InitPreview(character.spriteName));
        InitSkillsStrings(character.initialSkillsData);
    }

    private IEnumerator InitPreview(string spriteName)
    {
        characterPreview = previewUIItemSet.InstantiateItem(spriteName);
        // ждем пока сработает расстановка элементов от layout group
        yield return new WaitForEndOfFrame();

        previewImage.texture = characterPreview.GetPreviewTexture((int)previewImage.rectTransform.rect.width, (int)previewImage.rectTransform.rect.height);
    }

    protected void InitSkillsStrings(List<CharacterStateSkillData> skillsData)
    {
        foreach (CharacterStateSkillData skillData in skillsData)
        {
            SkillStringUIItem skillString = Instantiate(skillsStringsPrefab, infoPanel);
            skillString.Init(skillData);
            skillString.onPointerEnter += (CharacterStateSkillData skillState) => characterPreview.StartPreviewState(skillState.state);
            skillString.onPointerExit += (CharacterStateSkillData _) => characterPreview.StopPreviewState();
            skillsStrings.Add(skillString);
        }
    }

    protected void HideSkillsStrings()
    {
        foreach (SkillStringUIItem skillString in skillsStrings)
        {
            Destroy(skillString.gameObject);
        }
        skillsStrings.Clear();
    }
}
