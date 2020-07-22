using UnityEditor;
using UnityEngine;

public class CharacterStatesManager : MonoBehaviour
{
    [SerializeField] private Animator characterAnimator;

    [SerializeField] private CharacterToolsManager toolsManager;

    [SerializeField] private CharacterSkillsManager skillsManager;

    [SerializeField] private CharacterStateData idleState;

    [HideInInspector]
    public CharacterState currentState;

    private void Start()
    {
        SetState(idleState);
    }

    public void SetState(CharacterStateData stateData, Vector2 stateActionPosition = default)
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
        if (currentState != null)
        {
            currentState.OnEnd();
        }
        currentState = new CharacterState(stateData, skillsManager.GetStateSkill(stateData), characterAnimator, toolsManager);
        StartCoroutine(currentState.OnStart(transform.position, stateActionPosition));
    }
}

[CustomEditor(typeof(CharacterStatesManager))] [CanEditMultipleObjects]
public class CharacterStateMachineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CharacterStatesManager stateMachine = (target as CharacterStatesManager);
        if (stateMachine.currentState != null)
        {
            EditorGUILayout.TextField("Текущее состояние : ", stateMachine.currentState.Name);
        }
    }
}

