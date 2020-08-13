using System.Collections.Generic;
using UnityEngine.Experimental.U2D.Animation;


public enum ToolCode
{
    None, Default, Bag, Pick, Shovel
}

public class CharacterToolsManager : CharacterManager
{
    public ToolCode defaultTool;
    public SpriteResolver spriteResolver;
    public List<ToolEquipData> toolsEquipData = new List<ToolEquipData>();

    public void SetDefaultTool(ToolCode toolCode)
    {
        defaultTool = toolCode;
    }

    public void ChangeToolMode(ToolCode toolCode, bool toolMode)
    {
        if (toolCode == ToolCode.Default)
            toolCode =  defaultTool;
        ToolEquipData toolData = toolsEquipData.Find(toolEquipData => toolEquipData.toolCode == toolCode);
        if (toolData != null)
        {
            if (toolData.isToUseSpriteSwitch)
            {
                spriteResolver.SetCategoryAndLabel(spriteResolver.GetCategory(), toolMode ? toolData.spriteResolveLabel : "Main");
            }
            else
            {
                toolData.ChangeMode(toolMode);
            }
        }
    }
    
    public void HideTool(ToolCode toolCode)
    {
        if (toolCode != ToolCode.None)
            ChangeToolMode(toolCode, false);
    }
    public void ApplyTool(ToolCode toolCode)
    {
        foreach (ToolEquipData toolEquipData in toolsEquipData)
        {
            if (toolCode != toolEquipData.toolCode && !toolEquipData.isToUseSpriteSwitch)
                toolEquipData.Destroy();
        }
        if (toolCode != ToolCode.None)
            ChangeToolMode(toolCode, true);
    }
}
