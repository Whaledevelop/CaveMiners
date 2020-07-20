using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

[System.Serializable]
public class PrefabInstantiateData
{
    public GameObject prefab;
    public Transform parent;
    [HideInInspector]
    public GameObject instance;

    public bool IsActive => instance != null && instance.activeSelf;

    public void ChangeMode(bool mode)
    {
        if (mode)
            Instantiate();
        else
            Destroy();
    }

    public GameObject Instantiate()
    {
        if (!IsActive)
            instance = Object.Instantiate(prefab, parent);
        return instance;

    }

    public void Destroy()
    {
        if (IsActive)
            Object.Destroy(instance);
    }
}

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

public class MinerTools : MonoBehaviour
{
    public SpriteResolver spriteResolver;

    public Animator animator;

    public List<ToolEquipData> toolsEquipData = new List<ToolEquipData>();

    private bool bagMode;
    private bool isMining;


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
    public void HideTool(string toolCodeString)
    {
        ToolCode toolCode = (ToolCode)System.Enum.Parse(typeof(ToolCode), toolCodeString, true);
        if (toolCode != default)
        {
            ChangeToolMode(toolCode, false);
        }        
    }
    public void ApplyTool(string toolCodeString)
    {
        ToolCode toolCode = (ToolCode)System.Enum.Parse(typeof(ToolCode), toolCodeString, true);
        if (toolCode != default)
        {
            foreach (ToolEquipData toolEquipData in toolsEquipData)
            {
                if (toolCode != toolEquipData.toolCode && !toolEquipData.isToUseSpriteSwitch)
                    toolEquipData.Destroy();
            }
            ChangeToolMode(toolCode, true);
        }
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spriteResolver.SetCategoryAndLabel("CaveMiner" + 1, spriteResolver.GetLabel());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            spriteResolver.SetCategoryAndLabel("CaveMiner" + 2, spriteResolver.GetLabel());
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            spriteResolver.SetCategoryAndLabel("CaveMiner" + 3, spriteResolver.GetLabel());
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            spriteResolver.SetCategoryAndLabel("CaveMiner" + 4, spriteResolver.GetLabel());
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            spriteResolver.SetCategoryAndLabel("CaveMiner" + 5, spriteResolver.GetLabel());
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeMiningMode(!isMining);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            bagMode = !bagMode;
            ChangeToolMode(ToolCode.Bag, bagMode);
        }
    }


    
    private void ChangeMiningMode(bool newMode)
    {
        if (isMining != newMode)
        {
            isMining = newMode;
            ChangeToolMode(ToolCode.Pick, isMining);
            animator.SetBool("Mining", isMining);
        }
    }
}
