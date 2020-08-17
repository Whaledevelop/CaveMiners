using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StartMainData", menuName = "ScriptableObjects/StartMainData")]
public class StartMainData : ScriptableObject
{
    public bool isGridCanvasTurnedOn;
    public CharacterInitialData[] chosenCharacters;
}