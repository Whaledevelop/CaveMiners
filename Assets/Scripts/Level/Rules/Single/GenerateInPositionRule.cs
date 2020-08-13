using UnityEngine;

[CreateAssetMenu(fileName = "GenerateInPositionRule", menuName = "LevelGeneratorRules/GenerateInPositionRule")]
public class GenerateInPositionRule : GenerateSingleRule
{
    public TilesGroup tilesGroup;

    public override void Init(LevelSettings levelSettings)
    {
        base.Init(levelSettings);
        tilesGroup.Init(levelXRange, levelYRange);
        LevelSettings.GeneratedTilesCount countObj = levelSettings.tilesCount.Find(tileCount => tileCount.generateRule == this);
        if (countObj != null)
            tilesGroup.ApplyDirectionToCount(countObj.count);        
    }

    protected override bool CheckPosition(int x, int y)
    {
        return tilesGroup.CheckIfGroupPosition(x, y);
    }
}