using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharacterUIItem : UIItem<CharacterInitialData, Transform, int>, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Text nameText;

    [SerializeField] private SkillStringUIItem skillsStringsPrefab;

    [SerializeField] private RectTransform infoPanel;

    [SerializeField] private Image image;

    [SerializeField] private Color hoverColor;

    [SerializeField] private Color chooseColor;    

    [SerializeField] private CharacterStateData idleState;

    [SerializeField] private CharacterStateData greetState;

    [SerializeField] private CharacterPreview characterPreviewPrefab;

    [SerializeField] private RawImage previewImage;

    [SerializeField] private Text historyTextPrefab;

    [HideInInspector] public bool isChosen;    

    [HideInInspector] public CharacterInitialData character;

    private Color defaultColor;

    private CharacterPreview characterPreview;

    public Action<CharacterInitialData> onClickCharacter;

    private List<SkillStringUIItem> skillsStrings = new List<SkillStringUIItem>();

    private Text historyText;

    public void Start()
    {
        defaultColor = image.color;
    }

    public override void Init(CharacterInitialData setupData, Transform previewParent, int index)
    {
        character = setupData;
        nameText.text = setupData.name;
        StartCoroutine(InitPreview(previewParent, index));
        InitSkillsStrings(character.initialSkillsData);
    }

    private IEnumerator InitPreview(Transform previewParent, int index)
    {
        characterPreview = Instantiate(characterPreviewPrefab, previewParent);
        characterPreview.Init(character.spriteName);
        Vector3 positionInGrid = characterPreview.transform.position;
        positionInGrid.x = characterPreview.SpriteSize.x * index;
        characterPreview.transform.position = positionInGrid;

        // ждем пока сработает расстановка элементов от layout group
        yield return new WaitForEndOfFrame();

        previewImage.texture = characterPreview.GetPreviewTexture((int)previewImage.rectTransform.rect.width, (int)previewImage.rectTransform.rect.height);
    }

    private void InitSkillsStrings(List<CharacterStateSkillData> skillsData)
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

    private void HideSkillsStrings()
    {
        foreach(SkillStringUIItem skillString in skillsStrings)
        {
            Destroy(skillString.gameObject);
        }
        skillsStrings.Clear();
    }

    public void OnClickInfo()
    {
        Debugger.LogIEnumerable(skillsStrings);
        if (skillsStrings.Count > 0)
        {
            HideSkillsStrings();
            historyText = Instantiate(historyTextPrefab, infoPanel);
            historyText.text = character.description;
        }
        else
        {
            if (historyText != null)
                Destroy(historyText.gameObject);
            InitSkillsStrings(character.initialSkillsData);
        }
    }

    public void ChangeChooseMode(bool isChosen)
    {
        this.isChosen = isChosen;
        image.color = isChosen ? chooseColor : defaultColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color currentColor = image.color;
        currentColor.a = 1f;
        image.color = currentColor;
        characterPreview.StartPreviewState(greetState);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color currentColor = image.color;
        currentColor.a = isChosen ? 0.5f : defaultColor.a;
        image.color = currentColor;
        characterPreview.StopPreviewState();
    }
 

    public void OnPointerDown(PointerEventData eventData)
    {
        onClickCharacter?.Invoke(character);
    }
}