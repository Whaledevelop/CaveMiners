using UnityEngine;

[CreateAssetMenu(fileName = "GenerateInPositionRule", menuName = "LevelGeneratorRules/GenerateInPositionRule")]
public class GenerateInPositionRule : GenerateSingleRule
{
    public TilesGroup tilesGroup;

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