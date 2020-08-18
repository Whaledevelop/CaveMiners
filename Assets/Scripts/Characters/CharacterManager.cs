using UnityEngine;
using System.Collections;

/// <summary>
/// Обработчик определенного функционала персонажа
/// </summary>
public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public Character character;
    public CharacterInitialData characterData => character.characterData;

    public virtual void Init(Character character)
    {
        this.character = character;
    }
}
