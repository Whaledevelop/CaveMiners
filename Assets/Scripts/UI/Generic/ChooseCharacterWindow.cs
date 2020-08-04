using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChooseCharacterWindow : MonoBehaviour
{
    [SerializeField] private CharacterInitialData[] charactersData;

    [SerializeField] private CharacterUIItem itemPrefab;

    [SerializeField] private Transform charactersParent;

    [SerializeField] private int maxActiveCharacters;

    private List<CharacterUIItem> instantiatedItems = new List<CharacterUIItem>();

    private List<CharacterUIItem> chosenItems = new List<CharacterUIItem>();

    public void Init()
    {
        foreach (CharacterInitialData characterData in charactersData)
        {
            CharacterUIItem item = Instantiate(itemPrefab, charactersParent);
            item.Init(characterData);
            instantiatedItems.Add(item);
        }
    }

    public void OnChooseCharacter(CharacterInitialData character)
    {
        foreach(CharacterUIItem item in instantiatedItems)
        {
            if (item.character == character)
            {
                if (!chosenItems.Exists(i => i.character == character))
                {
                    item.ChangeChooseMode(true);
                    chosenItems.Add(item);
                    if (chosenItems.Count > maxActiveCharacters)
                    {
                        chosenItems[0].ChangeChooseMode(false);
                        chosenItems.RemoveAt(0);
                        break;
                    }
                }
            }
        }
    }

    public void OnUnchooseCharacter(CharacterInitialData character)
    {
        if (GetItem(character, out CharacterUIItem item))
        {
            item.ChangeChooseMode(false);
            chosenItems.Remove(item);
        }            
    }

    private bool GetItem(CharacterInitialData character, out CharacterUIItem item)
    {
        item = instantiatedItems.Find(i => i.character == character);
        return item != null;
    }
}

