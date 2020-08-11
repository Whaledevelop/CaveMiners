using UnityEngine;

[CreateAssetMenu(fileName = "GenerateInPositionSingleRule", menuName = "LevelGeneratorRules/GenerateInPositionSingleRule")]
public class GenerateInPositionSingleRule : GenerateSingleRule
{
    public enum Orientation { Middle, MiddleLeft, UpperLeft }

    public Orientation orientation;
    public Vector2Int startPositionLocal;
    public Vector2Int direction;
    public bool isRect;

    protected override bool CheckPosition(int x, int y, RangeInt levelXRange, RangeInt levelYRange)
    {
        Vector2Int localZero = DefineLocaZero(levelXRange, levelYRange);
        Vector2Int startPosition = localZero + startPositionLocal;

        RangeInt formXRange = new RangeInt(startPosition.x, startPosition.x + direction.x);

        if (formXRange.IsInRange(x))
        {
            RangeInt formYRange = new RangeInt(startPosition.y, startPosition.y + direction.y);
            if (isRect)
            {
                return formYRange.IsInRange(y);
            }
            else
            {
                if (x == startPosition.x)
                    return formYRange.IsInRange(y);
                else
                {
                    // Отрисовываем только боковые тайлы, без центра
                    return y == startPosition.y;
                }

            }
        }
        return false;
    }

    private Vector2Int DefineLocaZero(RangeInt xRange, RangeInt yRange)
    {
        switch (orientation)
        {
            default:
            case Orientation.MiddleLeft: return new Vector2Int(xRange.from, yRange.Average);

        }
    }
}