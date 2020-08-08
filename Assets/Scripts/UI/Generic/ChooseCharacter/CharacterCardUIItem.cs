using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CharacterCardUIItem : CharacterUIItem, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Image image;

    [SerializeField] private Color hoverColor;

    [SerializeField] private Color chooseColor;    

    [SerializeField] private CharacterStateData greetState;

    [HideInInspector] public bool isChosen;

    private Color defaultColor;

    public Action<CharacterInitialData> onClickCharacter;

    public void Start()
    {
        defaultColor = image.color;
    }


    public void OnClickInfo()
    {
        //if (skillsStrings.Count > 0)
        //{
        //    HideSkillsStrings();
        //    //historyText = Instantiate(historyTextPrefab, infoPanel);
        //    //historyText.text = character.description;
        //}
        //else
        //{
        //    //if (historyText != null)
        //    //    Destroy(historyText.gameObject);
        //    InitSkillsStrings(character.initialSkillsData);
        //}
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