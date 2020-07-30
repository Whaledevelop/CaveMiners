using UnityEngine;

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

    public virtual void OnEnable()
    {
        if (!isMultisessional)
            value = defaultValue;
    }

    public virtual void OnDisable()
    {
        if (!isMultisessional)
            value = defaultValue;
    }
}
