﻿using System.Collections;
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



    public Dictionary<ToolCode, PrefabInstantiateData> usedTools = new Dictionary<ToolCode, PrefabInstantiateData>();


    void Start()
    {
        foreach(ToolEquipData toolInstanstiateData in toolsEquipData)
        {
            if (!usedTools.ContainsKey(toolInstanstiateData.toolCode))
                usedTools.Add(toolInstanstiateData.toolCode, toolInstanstiateData);
            else
                Debug.Log("Повтор данных для инициализации инструмента " + toolInstanstiateData.toolCode);
        }
    }

    public void ChangeToolMode(ToolCode toolCode, bool newMode)
    {
        ToolEquipData toolData = toolsEquipData.Find(toolEquipData => toolEquipData.toolCode == toolCode);
        if (toolData != null)
        {
            if (newMode)
            {
                foreach (ToolEquipData toolEquipData in toolsEquipData)
                {
                    if (toolCode != toolEquipData.toolCode && !toolEquipData.isToUseSpriteSwitch)
                        toolEquipData.Destroy();
                }
            }

            if (toolData.isToUseSpriteSwitch)
            {
                spriteResolver.SetCategoryAndLabel(spriteResolver.GetCategory(), newMode ? toolData.spriteResolveLabel : "Main");
            }
            else
            {
                toolData.ChangeMode(newMode);
            }
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeDigingMode(!isDigging);
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

    private bool bagMode;
    private bool isDigging;
    private void ChangeDigingMode(bool newMode)
    {
        if (isDigging != newMode)
        {
            isDigging = newMode;            
            ChangeToolMode(ToolCode.Shovel, isDigging);
            animator.SetBool("Diging", isDigging);
        }

    }

    private bool isMining;
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