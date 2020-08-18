
using UnityEngine;

/// <summary>
/// Отдельное правило для генерации границ
/// </summary>
[CreateAssetMenu(fileName = "GenerateBordersRule", menuName = "LevelGeneratorRules/GenerateBordersRule")]
public class GenerateBordersRule : GenerateSingleRule
{
    protected override bool CheckPosition(int x, int y)
    {
        return x == levelXRange.from - 1 || x == levelXRange.to || y == levelYRange.from - 1 || y == levelYRange.to;
    }
}