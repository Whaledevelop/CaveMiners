using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class StartTestModeWindow : EditorWindow
{
    private bool isGridCanvasTurnedOn;
    public CharacterInitialData[] characters;

    [UnityEditor.MenuItem("Window/StartTestMode")]
    public static void ShowWindow()
    {
        StartTestModeWindow window = (StartTestModeWindow)GetWindow(typeof(StartTestModeWindow));
        window.Show();
        window.characters = new CharacterInitialData[3] { null, null, null };
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("Запуск Main сцены в обход Menu");

        GUILayout.Space(10);
        isGridCanvasTurnedOn = EditorGUILayout.Toggle("Координаты клеток", isGridCanvasTurnedOn);

        GUILayout.Space(10);
        GUILayout.Label("Выбранные персонажи");
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i] = (CharacterInitialData)EditorGUILayout.ObjectField(characters[i], typeof(CharacterInitialData), false);
        }        

        GUILayout.Space(20);
        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("Остановка PlayMode"))
            {
                EditorApplication.ExitPlaymode();
            }
        }
        else
        {
            if (GUILayout.Button("Запуск PlayMode"))
            {
                EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");

                SetStartData();

                EditorApplication.EnterPlaymode();
            }
        }
    }

    private void SetStartData()
    {
        StartMainData startMainData = (StartMainData)Resources.FindObjectsOfTypeAll(typeof(StartMainData))[0];

        if (startMainData != null)
        {
            startMainData.isGridCanvasTurnedOn = isGridCanvasTurnedOn;
            startMainData.chosenCharacters = characters;
        }            
        else
            Debug.Log("Не найден StartMainData");
    }
}