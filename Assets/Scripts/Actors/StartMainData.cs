using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Настройки прохождения для Main сцены. Нужно для запуска из эдитора, в обычном прохождении эти данные получаются 
/// из предыдущих сцены или не нужны не в тестовом режиме
/// </summary>
[CreateAssetMenu(fileName = "StartMainData", menuName = "ScriptableObjects/StartMainData")]
public class StartMainData : ScriptableObject
{
    public bool isGridCanvasTurnedOn;
    public CharacterInitialData[] chosenCharacters;
}