using System.Collections;
using UnityEngine;

public class CharacterState
{
    public CharacterStateData stateData;

    private Animator animator;

    private CharacterToolsManager toolManager;

    private float stateSkill;

    public string Name => stateData.stateName;

    public CharacterState(CharacterStateData stateData, float stateSkill, Animator animator, CharacterToolsManager toolManager)
    {
        this.stateData = stateData;
        this.stateSkill = stateSkill;
        this.animator = animator;
        this.toolManager = toolManager;
    }

    public IEnumerator OnStart(Vector2 characterPosition, Vector2 stateActionPosition)
    {
        stateData.OnStart(animator, toolManager, characterPosition, stateActionPosition);
        yield break;                    
    }

    public IEnumerator OnEnd()
    {
        yield break;
    }
}