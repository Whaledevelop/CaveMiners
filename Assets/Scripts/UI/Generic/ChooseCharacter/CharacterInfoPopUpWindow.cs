using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoPopUpWindow : MonoBehaviour
{
    [SerializeField] private CharacterDetailedInfoUIItem characterDetailedPrefab;

    [SerializeField] private Transform detailedInfoParent;

    private CharacterDetailedInfoUIItem characterDetailed;

    public void InitCharacterDetailedInfoItem(CharacterInitialData character)
    {
        characterDetailed = Instantiate(characterDetailedPrefab, detailedInfoParent);
        characterDetailed.Init(character);
    }

    public void OnCloseWindow()
    {
        Destroy(characterDetailed.gameObject);
    }
}
