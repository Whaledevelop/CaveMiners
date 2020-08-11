
using UnityEngine;

[CreateAssetMenu(fileName = "GenerateBordersRule", menuName = "LevelGeneratorRules/GenerateBordersRule")]
public class GenerateBordersRule : GenerateRule
{
    protected override bool DefineIfTileObservesRule(int x, int y, RangeInt xRange, RangeInt yRange)
    {
        return x == xRange.from - 1 || x == xRange.to || y == yRange.from - 1 || y == yRange.to;
    }
}