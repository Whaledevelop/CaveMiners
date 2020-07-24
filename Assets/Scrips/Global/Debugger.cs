using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Debugger
{
    public static bool IsLogging = true;

    public static void Log(object logObject, string color = "yellow")
    {
        if (IsLogging)
        {
            if (logObject == null)
                Debug.Log("<color=red>NULL</color>");
            else
                Debug.Log("<color=" + color + ">" + logObject + "</color>");
        }

    }

    public static void LogBool(bool boolValue, string description = null)
    {
        if (IsLogging)
        {
            description = description != null ? description : (boolValue ? "true" : "false");
            Debug.Log("<color=" + (boolValue ? "green" : "red") + ">" + description + "</color>");
        }
    }

    public static void LogIEnumerable<T>(IEnumerable<T> enumerable, string prefix = null, string color = "yellow")
    {
        LogIEnumerable(enumerable, prefix, false, color);
    }

    public static void LogMethod(string methodName, params object[] methodParams)
    {
        if (IsLogging)
            Debug.Log("[" + methodName + "] " + string.Join(", ", methodParams));
    }

    public static void LogIEnumerable<T>(IEnumerable<T> enumerable, string prefix, bool isInline, string color = "yellow")
    {
        if (IsLogging)
        {
            if (enumerable != null)
            {
                prefix = (string.IsNullOrEmpty(prefix) ? enumerable.GetType().Name : prefix) + "(" + enumerable.Count() + ")";
                if (isInline)
                {
                    string endString = enumerable.Count() == 0 
                        ? "<color=orange>" + prefix + " - пустой</color>" 
                        : "<color=" + color + ">" + prefix + " : </color>";
                    endString += string.Join(" ; ", enumerable);
                    Debug.Log(endString);
                }
                else
                {
                    if (enumerable.Count() == 0)
                    {
                        Debug.Log("<color=orange>" + prefix + " - пустой</color>");
                    }
                    else
                    {
                        Debug.Log("<color=" + color + ">" + prefix + " : </color>");
                        foreach (T elem in enumerable)
                        {
                            Debug.Log(elem);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("<color=red>IEnumerable null</color>");
            }
        }
    }
}


