using System;
using UnityEngine;

[Serializable]
public class TilesGroup
{
    public enum Orientation { Middle, MiddleLeft, MiddleRight, LowerLeft }

    [SerializeField] private Orientation orientation;
    [SerializeField] private Vector2Int startPositionLocal;
    [SerializeField] private Vector2Int direction;
    [SerializeField] private bool isRect;

    [NonSerialized] private Vector2Int localZero;
    [NonSerialized] private Vector2Int startPositionGlobal;
    [NonSerialized] private RangeInt formXRange;
    [NonSerialized] private RangeInt formYRange;

    // Можно создавать через едитор, можно в коде
    public TilesGroup(Orientation orientation, Vector2Int startPositionLocal, Vector2Int direction, bool isRect)
    {
        this.orientation = orientation;
        this.startPositionLocal = startPositionLocal;
        this.direction = direction;
        this.isRect = isRect;
    }

    public void ApplyDirectionToCount(int count)
    {
        if (isRect)
        {
            // TODO : освоить под квадратные 
        }
        else
        {
            // По x вытягиваем
            direction.x = count - direction.y - 1;
        }
    }

    public void Init(RangeInt levelXRange, RangeInt levelYRange)
    {
        localZero = DefineLocaZero(levelXRange, levelYRange);
        startPositionGlobal = localZero + startPositionLocal;
        formXRange = new RangeInt(startPositionGlobal.x, startPositionGlobal.x + direction.x);
        formYRange = new RangeInt(startPositionGlobal.y, startPositionGlobal.y + direction.y);
    }

    public bool CheckIfGroupPosition(int x, int y)
    {
        if (formXRange.IsInRange(x))
        {
            if (!isRect && x != startPositionGlobal.x)
            {
                // Отрисовываем только боковые тайлы, без центра
                return y == startPositionGlobal.y;
            }
            else
            {
                
                return formYRange.IsInRange(y);
            }
        }
        else
            return false;
    }

    private Vector2Int DefineLocaZero(RangeInt levelXRange, RangeInt levelYRange)
    {
        switch (orientation)
        {
            case Orientation.MiddleLeft: return new Vector2Int(levelXRange.from, levelYRange.Average);
            case Orientation.MiddleRight: return new Vector2Int(levelXRange.to, levelYRange.Average);
            case Orientation.LowerLeft: return new Vector2Int(levelXRange.from, levelYRange.from);
            case Orientation.Middle:
            default:
                return new Vector2Int(levelXRange.Average, levelYRange.Average);

        }
    }
}
