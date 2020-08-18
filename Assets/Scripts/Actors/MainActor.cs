using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Скрипт, инициализирующий всё необходимое в Main сцене. Он же обрабатывает основные события вызываемые
/// при прохождении
/// </summary>
public class MainActor : MonoBehaviour
{
    [SerializeField] private GameEvent gameOverEvent;

    [SerializeField] private LevelGenerator levelGenerator;

    // Базовые настройки уровня. Сейчас - это обновляемые настройки, т.е. которые меняются от уровня к уровню,
    // но также можно  будет сделать указание конкретных настройки для конкретного уровня. 
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

        Debugger.IsLogging = startMainData.isDebugTurned;
#endif

        levelGenerator.GenerateLevel(levelSettings);
        if (LevelCount == 1)
        {
            moneyVariable.Value = levelSettings.startMoney;
        }
    }

    public void OnChangeMoney(int money)
    {
        // Игра заканчивается, когда кончаются деньги
        if (money <= 0)
        {
            gameOverEvent.Raise();
        }            
    }

    public void OnBase()
    {
    }
    
    public void OnLevelPassed()
    {
        // Обновляем настройки для следующего уровняЫ
        levelSettings.Upgrade();
        // Обновляем сцену, при этом заново загружается уровень
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
