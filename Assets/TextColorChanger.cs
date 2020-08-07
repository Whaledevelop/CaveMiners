using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextColorChanger : MonoBehaviour
{
    [SerializeField] private Text text;

    public void ChangeColor(string stringColor)
    {
        text.color = Utils.GetColor(stringColor);
    }
}
