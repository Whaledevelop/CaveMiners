using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Скрипт, инициализирующий всё необходимое в Menu сцене
/// </summary>
public class MenuActor : MonoBehaviour
{
    [SerializeField] private GameEvent startPickingEvent;

    public void Start()
    {
        startPickingEvent.Raise();
    }

    public void OnCharactersChosen()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
