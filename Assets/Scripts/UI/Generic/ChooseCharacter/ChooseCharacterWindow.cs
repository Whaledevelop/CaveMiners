using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChooseCharacterWindow : MonoBehaviour
{
    [SerializeField] private CharacterInitialData[] charactersData;

    [SerializeField] private CharacterCardUIItem characterItemPrefab;

    [SerializeField] private RectTransform charactersParent;    

    [SerializeField] private Transform characterPreviewParent;

    [SerializeField] private int maxActiveCharacters;

    [SerializeField] private GameEvent onAllCharactersChosen;

    [SerializeField] private GameEvent onTryingToChooseLessCharacters;

    private List<CharacterCardUIItem> instantiatedItems = new List<CharacterCardUIItem>();

    [SerializeField] private CharacterInitialDataSet chosenCharacters;

    public void Start()
    {
        for (int i = 0; i < charactersData.Length; i++)
        {
            CharacterCardUIItem item = Instantiate(characterItemPrefab, charactersParent);
            item.Init(charactersData[i]);
            item.onClickCharacter += OnClickCharacterItem;
            instantiatedItems.Add(item);
        }
    }

    public void OnClickCharacterItem(CharacterInitialData character)
    {
        foreach(CharacterCardUIItem item in instantiatedItems)
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
                        CharacterCardUIItem oldestChosenCharacterUI = instantiatedItems.Find(i => i.character == oldestChosenCharacter);
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

