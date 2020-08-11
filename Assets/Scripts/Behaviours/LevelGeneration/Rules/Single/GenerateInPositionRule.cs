using System;
using UnityEngine;


[Serializable]
public struct TilesGroup
{
    public enum Orientation { Middle, MiddleLeft, MiddleRight, LowerLeft }

    [SerializeField] private Orientation orientation;
    [SerializeField] private Vector2Int startPositionLocal;
    [SerializeField] private Vector2Int direction;
    [SerializeField] private bool isRect;

    // Можно создавать через едитор, можно в коде
    public TilesGroup(Orientation orientation, Vector2Int startPositionLocal, Vector2Int direction, bool isRect)
    {
        this.orientation = orientation;
        this.startPositionLocal = startPositionLocal;
        this.direction = direction;
        this.isRect = isRect;
    }

    public bool CheckIfGroupPosition(int x, int y, RangeInt levelXRange, RangeInt levelYRange)
    {
        Vector2Int localZero = DefineLocaZero(levelXRange, levelYRange);
        Vector2Int startPosition = localZero + startPositionLocal;

        RangeInt formXRange = new RangeInt(startPosition.x, startPosition.x + direction.x);

        if (formXRange.IsInRange(x))
        {
            if (!isRect && x != startPosition.x)
            {
                // Отрисовываем только боковые тайлы, без центра
                return y == startPosition.y;
            }
            else
            {
                RangeInt formYRange = new RangeInt(startPosition.y, startPosition.y + direction.y);
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


[CreateAssetMenu(fileName = "GenerateInPositionRule", menuName = "LevelGeneratorRules/GenerateInPositionRule")]
public class GenerateInPositionRule : GenerateSingleRule
{
    public TilesGroup tilesGroup;

    protected override bool CheckPosition(int x, int y, RangeInt levelXRange, RangeInt levelYRange)
    {
        return tilesGroup.CheckIfGroupPosition(x, y, levelXRange, levelYRange);
    }
}