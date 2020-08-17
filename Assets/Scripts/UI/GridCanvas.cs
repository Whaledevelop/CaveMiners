﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GridCanvas : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Transform cellsParent;
    [SerializeField] private Text cellTextPrefab;

    [SerializeField] private RangeWithStep cellsXRange;
    [SerializeField] private RangeWithStep cellsYRange;

    /*
#if UNITY_EDITOR
    public void Start()
    {


        for(float x = cellsXRange.from; x <= cellsXRange.to; x += cellsXRange.step)
        {
            for (float y = cellsYRange.from; y <= cellsYRange.to; y += cellsYRange.step)
            {
                Text cellText = Instantiate(cellTextPrefab, cellsParent);

                cellText.transform.position = new Vector2(x, y);
                cellText.text = "(" + x + "," + y + ")";
            }
        }

    }
#endif
    */
}
