using System;
using UnityEngine;

[Serializable]
public class CharacterSkill
{
    public enum Code
    {
        None, Digging, Mining, Walking, FogObserver
    }

    public Code code;
    public int initialValue;
    [Range(0, 1)]
    public float learnability;

    private float currentLevelExp;

    [NonSerialized] private int value;
    public int Value
    {
        get => value == 0 ? initialValue : value;  // Чтобы не делать инициализацию
        private set
        {
            this.value = value;
        }
    }

    public void LearnSkill()
    {
        currentLevelExp += learnability;
        if (currentLevelExp >= 1)
        {
            Debug.Log("level up " + code + " from " + Value + " to " + (Value + 1));
            Value++;
            currentLevelExp = 0;            
        }
    }
}
