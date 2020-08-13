﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GenerateInAreaRule", menuName = "LevelGeneratorRules/GenerateInAreaRule")]
public class GenerateInAreaRule : GenerateSingleRule
{
    [Header("Сколько экземпляров")]
    [SerializeField] private int defaultInstanceCount = 1;

    [Header("Размер одного экземпляра")]
    [SerializeField] private Vector2Int instanceDirection;
    [SerializeField] private bool isInstanceRect;

    [Header("Может ли превышать разрешенную область (если размер больше)")]
    [SerializeField] private bool isExpandable;

    [HideInInspector] public Range xArea;
    [HideInInspector] public Range yArea;

    [NonSerialized] private List<TilesGroup> tilesGroups = new List<TilesGroup>();

    [NonSerialized] private int instancesCount;

    public override void Init(LevelSettings levelSettings)
    {
        tilesGroups.Clear();
        base.Init(levelSettings);
        GeneratedTilesCount countObj = levelSettings.tilesCount.Find(tileCount => tileCount.generateRule == this);
        if (countObj != null)
            instancesCount = countObj.Count;
        else
            instancesCount = defaultInstanceCount;

        for (int i = 0; i < instancesCount; i++)
        {
            int randomX = (int)Mathf.Round(levelXRange.Interval * xArea.Random);
            int randomY = (int)Mathf.Round(levelYRange.Interval * yArea.Random);
            TilesGroup tilesGroup = new TilesGroup(TilesGroup.Orientation.LowerLeft, new Vector2Int(randomX, randomY), instanceDirection, isInstanceRect);
            tilesGroup.Init(levelXRange, levelYRange);
            tilesGroups.Add(tilesGroup);
        }
    }

    protected override bool CheckPosition(int x, int y)
    {
        foreach(TilesGroup tilesGroup in tilesGroups)
        {
            if (tilesGroup.CheckIfGroupPosition(x, y))
                return true;
        }
        return false;

    }
}

[CustomEditor(typeof(GenerateInAreaRule)), CanEditMultipleObjects]
public class GenerateInAreaRuleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GenerateInAreaRule rule = target as GenerateInAreaRule;

        EditorGUILayout.Space();
        EditorGUILayout.PrefixLabel("X area, from none to full level");
        EditorGUILayout.MinMaxSlider(ref rule.xArea.from, ref rule.xArea.to, 0, 1);

        EditorGUILayout.PrefixLabel("Y area, from none to full level");
        EditorGUILayout.MinMaxSlider(ref rule.yArea.from, ref rule.yArea.to, 0, 1);
    }
}