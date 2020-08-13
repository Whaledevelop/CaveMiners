using System;

[Serializable]
public class GeneratedTilesCount
{
    public GenerateRule generateRule;
    public int initialCount;
    public int levelUpPlus;

    private int count;
    public int Count
    {
        get
        {
            if (count == 0)
                count = initialCount;
            return count;
        }
    }

    public GeneratedTilesCount(GenerateRule generateRule, int initialCount)
    {
        this.generateRule = generateRule;
        this.initialCount = count;
    }

    public void Upgrade()
    {
        count += levelUpPlus;
    }
}
