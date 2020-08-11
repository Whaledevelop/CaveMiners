using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private RangeInt xRange;
    [SerializeField] private RangeInt yRange;

    [SerializeField] private List<GenerateRule> generateRules = new List<GenerateRule>();

    public void Start()
    {
        Generate();
    }

    public void Generate()
    {
        for (int x = xRange.from - 1; x < xRange.to + 1; x++)
        { 
            for (int y = yRange.from - 1; y < yRange.to + 1; y++)
            {

                foreach (GenerateRule generateRule in generateRules)
                {
                    if (generateRule.HandlePosition(x, y, xRange, yRange))
                    {
                        if (generateRule.isSingleOnCell)
                            break;
                    }
                }
                    
            }
        }
    }
}
