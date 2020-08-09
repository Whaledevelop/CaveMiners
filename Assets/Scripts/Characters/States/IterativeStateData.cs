using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "IterativeStateData", menuName = "States/IterativeStateData")]
public class IterativeStateData : CharacterStateData
{
    public CharacterActionGameEvent iterationEvent;
    public float iterationsRange = 1;

    public EndExecutionCondition executionCondition;

    public int maxIterationsCount;

}