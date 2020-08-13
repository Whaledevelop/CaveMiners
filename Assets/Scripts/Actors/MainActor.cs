using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class MainActor : MonoBehaviour
{
    [SerializeField] private bool isGameEndable = true;

    [SerializeField] private GameEvent gameOverEvent;

    [SerializeField] private LevelGenerator levelGenerator;

    [SerializeField] private LevelSettings levelSettings;

    [SerializeField] private IntVariable money;

    public void Start()
    {
        levelGenerator.GenerateLevel(levelSettings);
        money.Value = levelSettings.startMoney;
    }

    public void OnChangeMoney(int money)
    {
        if (money <= 0 && isGameEndable)
        {
            gameOverEvent.Raise();
        }            
    }

    public void OnBase()
    {
        //Debug.Log("base");
    }
}
