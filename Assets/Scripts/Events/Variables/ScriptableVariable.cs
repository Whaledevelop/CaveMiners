using System;
using UnityEngine;

/// <summary>
/// ScriptableVariable - это тот же GameEvent, на который подписываются слушатели, но у него есть
/// своё значение какое-то
/// </summary>
public class ScriptableVariable<T> : GameEvent<T>
{
    [SerializeField] private string description;

    [SerializeField] private T defaultValue;

    [SerializeField] private bool isConst;

    private T value;

    public T Value
    {
        get => value;
        set
        {
            if (isConst)
            {
                Debugger.Log($"Trying to set const value for {name}");
                return;
            }
            this.value = value;
            Raise(value);
        }
    }

    protected virtual void OnEnable()
    {
        Value = defaultValue;
    }
}
