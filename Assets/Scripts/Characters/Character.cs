using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Главный контроллер персонажа
/// </summary>
public class Character : MonoBehaviour
{
    [SerializeField] private Highlighter highlighter;
    [SerializeField] private CharactersSet set;
    [SerializeField] private UnityEngine.U2D.Animation.SpriteResolver spriteResolver;
    [Header("Обработчики функционала персонажа")]
    [SerializeField] private List<CharacterManager> managers = new List<CharacterManager>();

    [HideInInspector] public CharacterInitialData characterData;

    public void Init(CharacterInitialData characterInitialData)
    {
        characterData = characterInitialData;
        InitView(characterData.spriteName);
        foreach (CharacterManager characterManager in managers)
        {
            characterManager.Init(this);
        }
    }

    public void InitView(string spriteName)
    {
        spriteResolver.SetCategoryAndLabel(spriteName, spriteResolver.GetLabel());
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
