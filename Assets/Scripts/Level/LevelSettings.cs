using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "ScriptableObjects/LevelSettings")]
public class LevelSettings : ScriptableObject
{
    [Serializable]
    public class GeneratedTilesCount
    {
        public GenerateRule generateRule;
        public int count;

        public GeneratedTilesCount(GenerateRule generateRule, int count)
        {
            this.generateRule = generateRule;
            this.count = count;
        }
    }

    public List<GeneratedTilesCount> tilesCount = new List<GeneratedTilesCount>();

    public RangeInt xLevelSizeRange;

    public RangeInt yLevelSizeRange;

    public int startMoney;

    [SerializeField] private CharactersSet chosenCharacters;

    [SerializeField] private GenerateRule placeForCharactersGenerateRule;

    public void OnEnable()
    {
        GeneratedTilesCount tileCount = tilesCount.Find(tile => tile.generateRule == placeForCharactersGenerateRule);
        if (tileCount == null)
            tilesCount.Add(new GeneratedTilesCount(placeForCharactersGenerateRule, chosenCharacters.Items.Count));
    }
}
