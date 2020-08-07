using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChooseCharacterWindow : MonoBehaviour
{
    [SerializeField] private CharacterInitialData[] charactersData;

    [SerializeField] private CharacterUIItem characterItemPrefab;

    [SerializeField] private RectTransform charactersParent;    

    [SerializeField] private Transform characterPreviewParent;

    [SerializeField] private int maxActiveCharacters;

    [SerializeField] private GameEvent onAllCharactersChosen;

    [SerializeField] private GameEvent onTryingToChooseLessCharacters;

    private List<CharacterUIItem> instantiatedItems = new List<CharacterUIItem>();

    [SerializeField] private CharacterInitialDataSet chosenCharacters;

    public void Start()
    {
        for (int i = 0; i < charactersData.Length; i++)
        {
            CharacterUIItem item = Instantiate(characterItemPrefab, charactersParent);
            item.Init(charactersData[i], characterPreviewParent, i);
            item.onClickCharacter += OnClickCharacterItem;
            instantiatedItems.Add(item);
        }
    }

    public void OnClickCharacterItem(CharacterInitialData character)
    {
        foreach(CharacterUIItem item in instantiatedItems)
        {
            if (item.character == character)
            {
                if (chosenCharacters.Items.Exists(i => i == character))
                {
                    item.ChangeChooseMode(false);
                    chosenCharacters.Remove(item.character);
                }
                else
                {
                    item.ChangeChooseMode(true);
                    chosenCharacters.Add(item.character);
                    if (chosenCharacters.Items.Count > maxActiveCharacters)
                    {
                        CharacterInitialData oldestChosenCharacter = chosenCharacters.Items[0];
                        CharacterUIItem oldestChosenCharacterUI = instantiatedItems.Find(i => i.character == oldestChosenCharacter);
                        oldestChosenCharacterUI.ChangeChooseMode(false);
                        chosenCharacters.Remove(oldestChosenCharacter);
                        break;
                    }
                }
            }
        }
    }

    public void OnConfirmChosenCharacters()
    {
        if (chosenCharacters.Items.Count == maxActiveCharacters)
            onAllCharactersChosen.Raise();
        else if (chosenCharacters.Items.Count < maxActiveCharacters)
            onTryingToChooseLessCharacters.Raise();
    }

}

