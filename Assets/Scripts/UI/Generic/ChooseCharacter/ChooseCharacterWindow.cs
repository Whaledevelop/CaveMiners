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

    private List<CharacterUIItem> instantiatedItems = new List<CharacterUIItem>();

    private List<CharacterUIItem> chosenItems = new List<CharacterUIItem>();

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
                if (chosenItems.Exists(i => i.character == character))
                {
                    item.ChangeChooseMode(false);
                    chosenItems.Remove(item);
                }
                else
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


    private bool GetItem(CharacterInitialData character, out CharacterUIItem item)
    {
        item = instantiatedItems.Find(i => i.character == character);
        return item != null;
    }
}

