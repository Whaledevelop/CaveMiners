﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuActor : MonoBehaviour
{
    [SerializeField] private GameEvent startPickingEvent;

    public void Start()
    {
        startPickingEvent.Raise();
    }
}
