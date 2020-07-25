using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class CharacterStatesManager : MonoBehaviour
{
    [SerializeField] private Animator characterAnimator;

    [SerializeField] private CharacterStatesManagersSet set;

    [SerializeField] private CharacterToolsManager toolsManager;

    [SerializeField] private CharacterSkillsManager skillsManager;

    [SerializeField] private CharacterStateData idleState;

    [HideInInspector] public CharacterState CurrentState;

    public Action onEndState;

    private void Start()
    {
        SetState(new CharacterActionData(this, idleState));
        set.Add(this);
    }

    private void OnDestroy()
    {
        set.Remove(this);
    }

    public void SetState(CharacterActionData stateAction)
    {
        if (CurrentState != null)
            EndState();
        CurrentState = new CharacterState(stateAction, skillsManager.GetStateSkill(stateAction.stateData), characterAnimator, toolsManager);
        CurrentState.OnStart();
    }

    public void EndState()
    {
        Debug.Log("EndState");
        if (CurrentState != null)
            CurrentState.OnEnd();
        CurrentState = null;
        onEndState?.Invoke();
        
    }
}

[CustomEditor(typeof(CharacterStatesManager))] [CanEditMultipleObjects]
public class CharacterStateMachineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CharacterStatesManager stateMachine = (target as CharacterStatesManager);
        if (stateMachine.CurrentState != null)
        {
            EditorGUILayout.TextField("Текущее состояние : ", stateMachine.CurrentState.Name);
        }
    }
}

