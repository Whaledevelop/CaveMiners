using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Окно выбора персонажей
/// </summary>
public class ChooseCharacterWindow : MonoBehaviour
{
    [SerializeField] private CharacterInitialData[] charactersData;

    [SerializeField] private CharacterCardUIItem characterItemPrefab;

    [SerializeField] private RectTransform charactersParent;    

    [SerializeField] private IntVariable maxActiveCharacters;

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
                // Если персонаж, на которой кликнули уже был выбран, то мы убираем его из списка выбранных, снимаем выделение
                if (chosenCharacters.Items.Exists(i => i == character))
                {
                    item.ChangeChooseMode(false);
                    chosenCharacters.Remove(item.character);
                }
                else
                {
                    // Если не был выбран, то выбираем
                    item.ChangeChooseMode(true);
                    chosenCharacters.Add(item.character);
                    // Если превышено максимальное количество выбранных персонажей, то снимаем выделение с персонажа, выбранного раньше всего
                    if (chosenCharacters.Items.Count > maxActiveCharacters.Value)
                    {
                        CharacterInitialData oldestChosenCharacter = chosenCharacters.Items[0];
                        CharacterCardUIItem oldestChosenCharacterUI = instantiatedItems.Find(i => i.character == oldestChosenCharacter);
                        oldestChosenCharacterUI.ChangeChooseMode(false);
                        chosenCharacters.Remove(oldestChosenCharacter);                        
                    }
                }
                break;
            }
        }
    }

    public void OnConfirmChosenCharacters()
    {
        if (chosenCharacters.Items.Count == maxActiveCharacters.Value)
            onAllCharactersChosen.Raise();
        else if (chosenCharacters.Items.Count < maxActiveCharacters.Value)
            onTryingToChooseLessCharacters.Raise();
    }

}

