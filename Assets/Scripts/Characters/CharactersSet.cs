using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Список всех персонажей
/// </summary>
[CreateAssetMenu(fileName = "CharactersSet", menuName = "Sets/CharactersSet")]
public class CharactersSet : RuntimeSet<Character>
{
    private Character activeCharacter;

    public Character ActiveCharacter
    {
        get => activeCharacter;
        set
        {
            if (activeCharacter == value)
            {
                return;
            }
            if (activeCharacter != null)
                activeCharacter.OnBecomeNotActive();

            activeCharacter = value;
            activeCharacter.OnBecomeActive();
        }
    }
}
