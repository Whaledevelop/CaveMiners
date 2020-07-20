using UnityEngine;


public class CharacterStateData : ScriptableObject
{
    public string animatorTriggerStart;
    public string animatorTriggerEnd;
    public string toolStringName;
    public GameEvent StartEvent;
    public GameEvent EndEvent;
}