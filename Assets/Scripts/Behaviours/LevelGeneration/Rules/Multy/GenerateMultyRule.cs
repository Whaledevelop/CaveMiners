
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GenerateMultyRule", menuName = "LevelGeneratorRules/GenerateMultyRule")]
public class GenerateMultyRule : GenerateRule
{
    [SerializeField] private List<GenerateRule> generateRules = new List<GenerateRule>();

    public override bool HandlePosition(int x, int y, RangeInt xRange, RangeInt yRange)
    {
        foreach(GenerateRule generateRule in generateRules)
        {
            if (generateRule.HandlePosition(x, y, xRange, yRange))
                return true;
        }
        return false;
    }
}