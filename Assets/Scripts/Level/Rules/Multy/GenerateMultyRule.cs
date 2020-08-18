
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Правило, включающее в себя несколько других правил, для генерации более сложных, комплексных объектов. Пока это только база,
/// но в дальнейшем так можно будет сделать пещеры, водопады и т.д.
/// </summary>
[CreateAssetMenu(fileName = "GenerateMultyRule", menuName = "LevelGeneratorRules/GenerateMultyRule")]
public class GenerateMultyRule : GenerateRule
{
    [SerializeField] private List<GenerateRule> generateRules = new List<GenerateRule>();

    public override void Init(LevelSettings levelSettings)
    {
        base.Init(levelSettings);
        foreach (GenerateRule generateRule in generateRules)
        {
            generateRule.Init(levelSettings);
        }
    }

    public override bool HandlePosition(int x, int y)
    {
        bool isPositionTaken = false;
        foreach(GenerateRule generateRule in generateRules)
        {
            if(generateRule.HandlePosition(x, y))
            {
                isPositionTaken = true;
                if (generateRule.isSingleOnCell)
                    break;
            }
        }
        return isPositionTaken;
    }
}