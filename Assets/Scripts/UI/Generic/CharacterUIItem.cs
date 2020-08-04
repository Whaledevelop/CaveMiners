using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterUIItem : UIItem<CharacterInitialData>, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Text nameText;

    [SerializeField] private TextsUIItem skillsStringsPrefab;

    [SerializeField] private Transform skillsParent;

    [SerializeField] private Image image;

    [SerializeField] private Color hoverColor;

    [SerializeField] private Color chooseColor;

    [SerializeField] private CharacterDataGameEvent chooseCharacterEvent;

    [SerializeField] private CharacterDataGameEvent unchooseCharacterEvent;

    [HideInInspector] public bool isChosen;

    [HideInInspector] public CharacterInitialData character;

    private Color defaultColor;    

    

    public void Start()
    {
        defaultColor = image.color;
    }

    public override void Init(CharacterInitialData setupData)
    {
        character = setupData;
        nameText.text = setupData.name;
        foreach (CharacterStateSkillData skillData in setupData.initialSkillsData)
        {
            Instantiate(skillsStringsPrefab, skillsParent).Init(new string[] { skillData.state.stateName, skillData.value.ToString() });
        }
    }

    public void ChangeChooseMode(bool chooseMode)
    {
        isChosen = chooseMode;
        image.color = isChosen ? chooseColor : defaultColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color currentColor = image.color;
        currentColor.a = 1f;
        image.color = currentColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color currentColor = image.color;
        currentColor.a = isChosen ? 0.5f : defaultColor.a;
        image.color = currentColor;
    }
 

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isChosen)
        {
            chooseCharacterEvent.Raise(character);
        }
        else
        {
            unchooseCharacterEvent.Raise(character);
        }
    }
}