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
    public float Interval => to - from;
}

[Serializable]
public struct RangeInt
{
    public int from;
    public int to;

    public int Average => (to + from) / 2;
    public int Interval => to - from;
}
