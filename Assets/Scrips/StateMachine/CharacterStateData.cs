using UnityEngine;


[CreateAssetMenu(fileName = "CharacterStateData", menuName = "ScriptableObjects/CharacterStateData")]
public class CharacterStateData : ScriptableObject
{
    public string stateName;
 
    public ToolCode toolCode;

    [Header("Переход на событие")]
    public string animatorTriggerStart;
    public Request startRequest;
    public bool areResolveParamsNeededInEvent;
    public GameEvent startEvent;

    [Header("Окончание события")]
    public GameEvent endEvent;
    public string animatorTriggerEnd;
}