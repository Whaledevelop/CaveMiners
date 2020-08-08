using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class RuntimeSet<T> : ScriptableObject
{
    [HideInInspector]
    public List<T> Items = new List<T>();

    public void Add(T item)
    {
        if (!Items.Contains(item))
            Items.Add(item);
    }

    public void Remove(T item)
    {
        if (Items.Contains(item))
            Items.Remove(item);
    }

    public virtual void OnDisable()
    {
        Items.Clear();
    }
}