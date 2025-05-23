﻿using System;
using System.Collections;
using UnityEngine;



[CreateAssetMenu(fileName = "CharacterState", menuName = "States/CharacterState")]
public class CharacterState : ScriptableObject
{
    public enum Period
    {
        None, Start, Execution, End
    }
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

    public virtual IEnumerator Execute()
    {
        yield return OnStart();

        // Выполнение

        yield return OnEnd();
    }

    public virtual IEnumerator OnStart()
    {
        UpdateView(Period.Start);
        yield break;
    }

    public virtual IEnumerator Cancel()
    {
        UpdateView(Period.End);
        yield break;
    }

    public virtual IEnumerator OnEnd()
    {
        UpdateView(Period.End);
        yield break;
    }

    public void UpdateView(Period period)
    {
        switch (period)
        {
            case Period.Start:
                SetTrigger(animator, animatorStartTrigger);
                toolsManager.ApplyTool(toolCode);
                break;
            case Period.End:
                SetTrigger(animator, animatorEndTrigger);
                toolsManager.HideTool(toolCode);
                break;
        }
    }

    private void SetTrigger(Animator animator, string trigger)
    {
        if (string.IsNullOrEmpty(trigger))
        {
            return;
        }
        // Иногда возникает ситуация, когда произходит вызов другой анимации, когда еще происходит вызов
        // предыдущей, тогда триггер остается включенным, что приводит к тому, что следующая попытка вызвать ту же 
        // анимацию не сработает. Поэтому снимаем все триггеры вручную
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
                animator.ResetTrigger(parameter.name);
        }
        animator.SetTrigger(trigger);
    }

    public override string ToString()
    {
        return stateName;
    }
}