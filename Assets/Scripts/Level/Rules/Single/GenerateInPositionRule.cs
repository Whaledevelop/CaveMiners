using UnityEngine;

/// <summary>
/// Правило для генерации тайла в определенной точке
/// </summary>
[CreateAssetMenu(fileName = "GenerateInPositionRule", menuName = "LevelGeneratorRules/GenerateInPositionRule")]
public class GenerateInPositionRule : GenerateSingleRule
{
    [SerializeField] private TilesGroup tilesGroup;

    public override void Init(LevelSettings levelSettings)
    {
        base.Init(levelSettings);
        tilesGroup.Init(levelXRange, levelYRange);
        int tilesCount = levelSettings.GetTilesCount(this);
        if (tilesCount != 0)
            tilesGroup.ApplyDirectionToCount(tilesCount);                   
    }

    protected override bool CheckPosition(int x, int y)
    {
        return tilesGroup.CheckIfGroupPosition(x, y);
    }
}