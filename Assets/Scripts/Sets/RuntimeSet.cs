using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

/// <summary>
/// Список, записанный в скриптабл объект + выполняемые для этого списка методы. Может использоваться как для MonoBehaviour, так и для ScriptableObjects.
/// Одна из замен Singleton, который спустя время превращается в по сути список списков с соответствующими методами для каждого. Так же списки в ScriptableObjects
/// решают вопрос с переходами между сцен - скриптабли данные не теряют.
/// Вдохновлены https://www.youtube.com/watch?v=raQ3iHhE_Kk&list=PLB8F3398G-ZsPa0piiMEglkbLSyRggTf8
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
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