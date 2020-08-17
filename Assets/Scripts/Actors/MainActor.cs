using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainActor : MonoBehaviour
{
    [SerializeField] private bool isGameEndable = true;

    [SerializeField] private GameEvent gameOverEvent;

    [SerializeField] private LevelGenerator levelGenerator;

    [SerializeField] private LevelSettings levelSettings;

    [SerializeField] private IntVariable moneyVariable;

    #region Для теста в эдиторе

    [SerializeField] private GridCanvas gridCanvas;
    [SerializeField] private StartMainData startMainData;
    [SerializeField] private CharacterInitialDataSet chosenCharacters;

    #endregion

    public int LevelCount => levelSettings.UpdateLevel;

    public void Start()
    {
#if UNITY_EDITOR
        if (startMainData.isGridCanvasTurnedOn)
            gridCanvas.ShowCanvas();

        if (chosenCharacters.Items.Count == 0 && startMainData.chosenCharacters.Length > 0)
            chosenCharacters.Items = startMainData.chosenCharacters.ToList();
#endif

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
