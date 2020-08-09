using System;
using UnityEngine;

[Serializable]
public struct RangeWithStep
{
    public float from;
    public float to;
    public float step;
}

[Serializable]
public struct Range
{
    public float from;
    public float to;

    public float Average => (to + from) / 2;
}
