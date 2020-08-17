using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// CharacterActionState - это состояние выполнения какого действия на карте
/// </summary>
[CreateAssetMenu(fileName = "CharacterActionState", menuName = "States/CharacterActionState")]
public class CharacterActionState : CharacterState
{
    [Header("Талант выполнения данного состояния")]
    public CharacterSkill.Code skillCode;
    [Header("Слой, соответствующий состоянию")]
    [Tooltip("Используется для определения состояния по слою объекта, на который был клик")]
    public LayerMask actionLayerMask;
    [Header("Приоритет состояния")]
    [Tooltip("При поиске пути будет выбрана клетка, на которое действие имеет бОльший приоритет")]
    public int priority;
    [Header("Цвет гизмо (для эдитора)")]
    public Color gizmosColor;
    [Header("Режим поворотов при выполнении состояния")]
    public RotationMode rotationMode;

    // Данные экземпляра
    [NonSerialized] protected CharacterActionHandler actionHandler;
    [NonSerialized] protected Rotator rotator;
    [NonSerialized] protected CharacterAction actionData;

    public virtual void InitInstance(Animator animator, CharacterToolsManager toolsManager, Rotator rotator, CharacterAction actionData, CharacterActionHandler actionHandler)
    {
        base.InitInstance(animator, toolsManager);
        this.rotator = rotator;
        this.actionData = actionData;
        this.actionHandler = actionHandler;
    }

    public override IEnumerator Execute()
    {
        yield return OnStart();
        yield return actionHandler.Execute(actionData);
        yield return OnEnd();
    }

    public override IEnumerator Cancel()
    {
        yield return actionHandler.Cancel();
        yield return base.Cancel();
    }

    public override IEnumerator OnStart()
    {
        yield return base.OnStart();
        rotator.Rotate(actionData.actionDirection, rotationMode);
    }
}
