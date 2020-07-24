using System;
using UnityEditor;
using UnityEngine;

public class CharacterStatesManager : MonoBehaviour
{
    [SerializeField] private Animator characterAnimator;

    [SerializeField] private CharacterStatesManagersSet set;

    [SerializeField] private CharacterToolsManager toolsManager;

    [SerializeField] private CharacterSkillsManager skillsManager;

    [SerializeField] private CharacterStateData idleState;

    [HideInInspector] public CharacterState currentState;

    public Action onEndState;

    private void Start()
    {
        SetState(idleState);
        set.Add(this);
    }

    private void OnDestroy()
    {
        set.Remove(this);
    }

    public void SetState(CharacterStateData stateData, Vector2 stateActionPosition = default)
    {
        currentState = new CharacterState(stateData, skillsManager.GetStateSkill(stateData), characterAnimator, toolsManager);
        StartCoroutine(currentState.OnStart(transform.position, stateActionPosition));
    }

    public void EndState()
    {        
        if (currentState != null)
            currentState.OnEnd();
        onEndState?.Invoke();
        currentState = null;
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

