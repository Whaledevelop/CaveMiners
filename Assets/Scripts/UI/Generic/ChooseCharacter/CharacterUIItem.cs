using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class CharacterUIItem : UIItem<CharacterInitialData, Transform, int>, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Text nameText;

    [SerializeField] private SkillStringUIItem skillsStringsPrefab;

    [SerializeField] private RectTransform skillsParent;

    [SerializeField] private Image image;

    [SerializeField] private Color hoverColor;

    [SerializeField] private Color chooseColor;    

    [SerializeField] private CharacterStateData idleState;

    [SerializeField] private CharacterStateData greetState;

    [SerializeField] private CharacterPreview characterPreviewPrefab;

    [SerializeField] private RawImage previewImage;

    [HideInInspector] public bool isChosen;    

    [HideInInspector] public CharacterInitialData character;

    private Color defaultColor;

    private CharacterPreview characterPreview;

    public Action<CharacterInitialData> onClickCharacter;

    public void Start()
    {
        defaultColor = image.color;
    }

    public override void Init(CharacterInitialData setupData, Transform previewParent, int index)
    {
        character = setupData;
        nameText.text = setupData.name;

        StartCoroutine(InitPreview(previewParent, index));
        
        foreach (CharacterStateSkillData skillData in setupData.initialSkillsData)
        {
            SkillStringUIItem skillString = Instantiate(skillsStringsPrefab, skillsParent);
            skillString.Init(skillData);
            skillString.onPointerEnter += characterPreview.PreviewState;
            skillString.onPointerExit += (CharacterStateSkillData _) => characterPreview.PreviewState(idleState);
        }
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
        Debug.Log(previewImage.rectTransform.rect);

        previewImage.texture = characterPreview.GetPreviewTexture((int)previewImage.rectTransform.rect.width, (int)previewImage.rectTransform.rect.height);
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
        characterPreview.PreviewState(greetState);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color currentColor = image.color;
        currentColor.a = isChosen ? 0.5f : defaultColor.a;
        image.color = currentColor;
        characterPreview.PreviewState(idleState);
    }
 

    public void OnPointerDown(PointerEventData eventData)
    {
        onClickCharacter?.Invoke(character);
    }
}