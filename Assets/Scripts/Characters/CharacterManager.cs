using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private Highlighter highlighter;
    [SerializeField] private CharacterTasksManager tasksManager;
    [SerializeField] private CharacterManagersSet set;

    [HideInInspector] public CharacterInitialData characterData;

    public void Init(CharacterInitialData characterInitialData)
    {
        characterData = characterInitialData;
    }

    public void OnEnable()
    {
        set.Add(this);
    }

    public void OnDisable()
    {
        set.Remove(this);
    }

    public void ExecuteTask(int layer, Vector2 position)
    {
        tasksManager.ExecuteTask(layer, position);
    }

    public void OnBecomeNotActive()
    {
        highlighter.SwapHighlightMode();
    }

    public void OnBecomeActive()
    {
        highlighter.SwapHighlightMode();
    }
}
