using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "GenerativeTilemapsSet", menuName = "Sets/GenerativeTilemapsSet")]
public class GenerativeTilemapsSet : RuntimeSet<GenerativeTilemap>
{
    public GenerativeTilemap FindByCode(GenerativeTilemapCode generativeTilemapCode)
    {
        return Items.Find(item => item.tilemapData == generativeTilemapCode);
    }
}