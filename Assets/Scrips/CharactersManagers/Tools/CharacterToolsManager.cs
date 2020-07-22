using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;



[System.Serializable]
public class ToolEquipData : PrefabInstantiateData
{
    public ToolCode toolCode;
    public bool isToUseSpriteSwitch = false;
    public string spriteResolveLabel;
}


public enum ToolCode
{ 
    None,
    Bag,
    Pick,
    Shovel
}

public class CharacterToolsManager : MonoBehaviour
{
    public SpriteResolver spriteResolver;

    public List<ToolEquipData> toolsEquipData = new List<ToolEquipData>();


    public void ChangeToolMode(ToolCode toolCode, bool toolMode)
    {
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

    // Для UnityEvent, который не работает с enum в качестве статического параметра
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


    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    spriteResolver.SetCategoryAndLabel("CaveMiner" + 1, spriteResolver.GetLabel());
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    spriteResolver.SetCategoryAndLabel("CaveMiner" + 2, spriteResolver.GetLabel());
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    spriteResolver.SetCategoryAndLabel("CaveMiner" + 3, spriteResolver.GetLabel());
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    spriteResolver.SetCategoryAndLabel("CaveMiner" + 4, spriteResolver.GetLabel());
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    spriteResolver.SetCategoryAndLabel("CaveMiner" + 5, spriteResolver.GetLabel());
        //}


        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    ChangeMiningMode(!isMining);
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    bagMode = !bagMode;
        //    ChangeToolMode(ToolCode.Bag, bagMode);
        //}
    }


    
    //private void ChangeMiningMode(bool newMode)
    //{
    //    if (isMining != newMode)
    //    {
    //        isMining = newMode;
    //        ChangeToolMode(ToolCode.Pick, isMining);
    //        animator.SetBool("Mining", isMining);
    //    }
    //}
}
