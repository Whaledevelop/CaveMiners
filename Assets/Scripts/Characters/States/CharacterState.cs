using System;
using System.Collections;
using UnityEngine;

public enum StateStage
{
    None, Start, Execution, End
}

[CreateAssetMenu(fileName = "CharacterState", menuName = "States/CharacterState")]
public class CharacterState : ScriptableObject
{
    // Общие данные
    public string stateName;
    public ToolCode toolCode;
    public string animatorStartTrigger;
    public string animatorEndTrigger;

    // Данные экземпляра
    [NonSerialized] protected Animator animator;
    [NonSerialized] protected CharacterToolsManager toolsManager;

    public virtual void InitInstance(Animator animator, CharacterToolsManager toolsManager)
    {
        this.animator = animator;
        this.toolsManager = toolsManager;
    }

    public virtual IEnumerator Start()
    {
        UpdateView(StateStage.Start, animator, toolsManager);
        yield break;
    }

    public virtual IEnumerator End()
    {
        UpdateView(StateStage.End, animator, toolsManager);
        yield break;
    }

    public void UpdateView(StateStage stateStage, Animator animator, CharacterToolsManager toolsManager)
    {
        switch (stateStage)
        {
            case StateStage.Start:
                SetTrigger(animator, animatorStartTrigger);
                toolsManager.ApplyTool(toolCode);
                break;
            case StateStage.End:
                SetTrigger(animator, animatorEndTrigger);
                toolsManager.HideTool(toolCode);
                break;
        }
    }

    private void SetTrigger(Animator animator, string trigger)
    {
        // Иногда возникает ситуация, когда произходит вызов другой анимации, когда еще происходит вызов
        // предыдущей, тогда триггер остается включенным, что приводит к тому, что следующая попытка вызвать ту же 
        // анимацию не сработает. Поэтому снимаем все триггеры вручную
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
                animator.ResetTrigger(parameter.name);
        }
        if (!string.IsNullOrEmpty(trigger))
        {
            animator.SetTrigger(trigger);
        }
    }
}