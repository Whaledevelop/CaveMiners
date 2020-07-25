﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct Interval
{
    public float from;
    public float to;
    public float step;
}


public class GridCanvas : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Transform cellsParent;
    [SerializeField] private Text cellTextPrefab;

    [SerializeField] private Interval cellsXInterval;
    [SerializeField] private Interval cellsYInterval;

    public void Start()
    {
        for(float x = cellsXInterval.from; x <= cellsXInterval.to; x += cellsXInterval.step)
        {
            for (float y = cellsYInterval.from; y <= cellsYInterval.to; y += cellsYInterval.step)
            {
                Text cellText = Instantiate(cellTextPrefab, cellsParent);

                cellText.transform.position = new Vector2(x, y);
                cellText.text = "(" + x + "," + y + ")";
            }
        }
    }
}