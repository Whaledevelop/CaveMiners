using UnityEditor;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    public Animator characterAnimator;

    public MinerTools characterToolsManager;

    public Rotator rotator;

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
        }

        CharacterState state = new CharacterState(stateData, characterAnimator, characterToolsManager);
        if (state.CheckIfStartAvailable(transform.position, new Vector2(rotator.RightLeftMultiplier, 0)))
        {
            if (currentState != null)
            {
                currentState.OnEnd();
            }
            currentState = state;
            currentState.OnStart();
        }
        else
        {
            state.OnStartNotAvailable();
        }
    }
}

[CustomEditor(typeof(CharacterStateMachine))] [CanEditMultipleObjects]
public class CharacterStateMachineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CharacterStateMachine stateMachine = (target as CharacterStateMachine);
        if (stateMachine.currentState != null)
        {
            EditorGUILayout.TextField("Текущее состояние : ", stateMachine.currentState.Name);
        }
    }
}

