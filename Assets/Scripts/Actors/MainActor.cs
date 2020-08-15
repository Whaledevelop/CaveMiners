using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MainActor : MonoBehaviour
{
    [SerializeField] private bool isGameEndable = true;

    [SerializeField] private GameEvent gameOverEvent;

    [SerializeField] private LevelGenerator levelGenerator;

    [SerializeField] private LevelSettings levelSettings;

    //[SerializeField] private GameEvent updateLevelEvent;

    [SerializeField] private IntVariable moneyVariable;

    public int LevelCount => levelSettings.UpdateLevel;

    public void Start()
    {
        levelGenerator.GenerateLevel(levelSettings);
        if (LevelCount == 1)
        {
            moneyVariable.Value = levelSettings.startMoney;
        }
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
    
    public void OnLevelPassed()
    {
        levelSettings.Upgrade();
        // Обновляем сцену, при этом заново загружается уровень
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
