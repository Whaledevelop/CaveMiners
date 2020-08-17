using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Вынужденный скрипт, т.к. в UnityEvent нельзя указать свойство с типом Color (или я чего-то не знаю)
/// </summary>
public class TextColorChanger : MonoBehaviour
{
    [SerializeField] private Text text;

    public void ChangeColor(string stringColor)
    {
        text.color = Utils.GetColor(stringColor);
    }
}
