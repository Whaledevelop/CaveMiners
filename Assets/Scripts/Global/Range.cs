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

    public float Random => UnityEngine.Random.Range(from, to);

    public bool IsInRange(float value) => value >= from && value <= to;
}

[Serializable]
public struct RangeInt
{
    public int from;
    public int to;

    public int Average => (to + from) / 2;
    public int Interval => to - from;

    public RangeInt(int from, int to)
    {
        if (to > from)
        {
            this.from = from;
            this.to = to;
        }
        else
        {
            this.from = to;
            this.to = from;
        }
    }

    public bool IsInRange(int value) => value >= from && value <= to;
    public bool IsInRange(float value) => value >= from && value <= to;

    public bool IsDefault => from == 0 && to == 0;

    public override string ToString()
    {
        return "from " + from + " to " + to;
    }
}
