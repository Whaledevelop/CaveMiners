using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class Character : MonoBehaviour
{
    [SerializeField] private Highlighter highlighter;
    [SerializeField] private CharactersSet set;
    [SerializeField] private SpriteResolver spriteResolver;

    [SerializeField] private List<CharacterManager> managers = new List<CharacterManager>();

    [HideInInspector] public CharacterInitialData characterData;

    public void Init(CharacterInitialData characterInitialData)
    {
        characterData = characterInitialData;
        spriteResolver.SetCategoryAndLabel(characterData.spriteName, spriteResolver.GetLabel());
        foreach(CharacterManager characterManager in managers)
        {
            characterManager.Init(this);
        }
    }

    public T GetManager<T>() where T : CharacterManager
    {
        return (T)managers.Find(manager => manager is T);
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
        GetManager<CharacterTasksManager>().ExecuteTask(layer, position);
    }

    public void OnBecomeNotActive()
    {
        highlighter.SwapHighlightMode();
    }

    public void OnBecomeActive()
    {
        highlighter.SwapHighlightMode();
    }

    public override string ToString()
    {
        return characterData.name + ", sprite : " + spriteResolver.GetCategory();
    }
}
