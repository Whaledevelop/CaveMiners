using UnityEngine;
using System.Collections;

public class CharacterStateMachine : MonoBehaviour
{
    public Animator characterAnimator;

    public MinerTools characterToolsManager;

    public CharacterStateData idleState;

    public CharacterState currentState;

    private void Start()
    {
        SetState(idleState);
    }
    public void SetState(CharacterStateData stateData)
    {
        if (currentState != null)
        {
            // Повторный вызов одного и того же стейта приводит к возврату к idle положению
            if (currentState.stateData == stateData && currentState.stateData != idleState)
            {
                SetState(idleState);
                return;
            }                
            else
                currentState.OnEnd();
        }

        currentState = new CharacterState(stateData, characterAnimator, characterToolsManager);
        currentState.OnStart();
    }
}
