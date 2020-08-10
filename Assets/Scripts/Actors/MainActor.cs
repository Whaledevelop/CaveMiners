using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.Rendering;

public class MainActor : MonoBehaviour
{
    [SerializeField] private bool isGameEndable = true;
    [SerializeField] private GameEvent gameOverEvent;

    [SerializeField] private CharacterInitialDataSet chosenCharacters;

    [SerializeField] private CharacterManager characterPrefab;

    [SerializeField] private Transform charactersParent;

    public void Start()
    {
        for(int i = 0; i < chosenCharacters.Items.Count; i++)
        {
            CharacterManager characterObject = Instantiate(characterPrefab, charactersParent);
            characterObject.transform.position = new Vector2(characterObject.transform.position.x + i * 0.1f, characterObject.transform.position.y);

            characterObject.Init(chosenCharacters.Items[i]);            
        }
    }

    public void OnChangeMoney(float money)
    {
        if (money <= 0 && isGameEndable)
        {
            gameOverEvent.Raise();
        }            
    }
}
