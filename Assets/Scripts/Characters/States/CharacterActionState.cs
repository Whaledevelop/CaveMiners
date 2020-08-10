using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "CharacterActionState", menuName = "States/CharacterActionState")]
public class CharacterActionState : CharacterState
{
    public string skillDescription;
    public LayerMask actionLayerMask;
    public int actionPriority;
    public Color gizmosColor;
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
