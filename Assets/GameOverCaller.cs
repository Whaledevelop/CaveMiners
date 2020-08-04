using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameOverCaller : MonoBehaviour
{
    [SerializeField] private bool isGameEndable = true;
    [SerializeField] private GameEvent gameOverEvent;

    public void OnChangeMoney(float money)
    {
        if (money <= 0 && isGameEndable)
        {
            gameOverEvent.Raise();
        }            
    }
}
