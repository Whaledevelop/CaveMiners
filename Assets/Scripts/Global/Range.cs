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

    public bool IsInRange(int value, bool fromInclusevly = true, bool toInclusevly = true)
    {
        bool isFromIn = fromInclusevly ? value >= from : value > from;
        bool isToIn = toInclusevly ? value <= to : value < to;
        return isFromIn && isToIn;
    }

    public override string ToString()
    {
        return "from " + from + " to " + to;
    }
}
