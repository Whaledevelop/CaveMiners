using UnityEngine;

/// <summary>
/// ScriptableVariable - это тот же GameEvent, на который подписываются слушатели, но у него есть
/// своё значение какое-то
/// </summary>
public class ScriptableVariable<T> : GameEvent<T>
{
    [SerializeField] private string description;

    [SerializeField] private T defaultValue;

    [SerializeField] private bool isMultisessional;

    private T value;

    public T Value
    {
        get => value;
        set
        {
            this.value = value;
            Raise(value);
        }
    }

    public virtual void OnDisable()
    {
        if (!isMultisessional)
            value = defaultValue;
    }
}
