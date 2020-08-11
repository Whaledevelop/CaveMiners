using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private LevelSettings levelSettings;

    [SerializeField] private List<GenerateRule> generateRules = new List<GenerateRule>();

    public void Generate()
    {
        foreach (GenerateRule generateRule in generateRules)
        {
            generateRule.Init(levelSettings);
        }
        for (int x = levelSettings.xLevelSizeRange.from - 1; x < levelSettings.xLevelSizeRange.to + 1; x++)
        { 
            for (int y = levelSettings.yLevelSizeRange.from - 1; y < levelSettings.yLevelSizeRange.to + 1; y++)
            {
                foreach (GenerateRule generateRule in generateRules)
                {
                    if (generateRule.HandlePosition(x, y))
                    {
                        if (generateRule.isSingleOnCell)
                            break;
                    }
                }                    
            }
        }
    }
}
