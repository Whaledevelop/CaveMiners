using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MainActor : MonoBehaviour
{
    [SerializeField] private bool isGameEndable = true;
    [SerializeField] private GameEvent gameOverEvent;

    [SerializeField] private CharacterInitialDataSet chosenCharacters;

    public void Start()
    {
        
    }

    public void OnChangeMoney(float money)
    {
        if (money <= 0 && isGameEndable)
        {
            gameOverEvent.Raise();
        }            
    }
}
