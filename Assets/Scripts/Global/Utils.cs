using UnityEngine;
using System.Collections;

public enum UIColor
{
    Black,
    Red,
    Green
}

public class Utils
{
    public static Color GetColor(string uiColorString)
    {
        return GetColor((UIColor)System.Enum.Parse(typeof(UIColor), uiColorString, true));
    }
    // Полезно для UnityEvent
    public static Color GetColor(UIColor uiColor)
    {
        switch(uiColor)
        {
            case UIColor.Green: return Color.green;
            case UIColor.Red:  return Color.red;
            default: case UIColor.Black: return Color.black;                
        }
    }
    public static Color RandomColor()
    {
        return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }

    public static int MaskToLayer(LayerMask layerMask) => (int)Mathf.Log(layerMask.value, 2);
}
