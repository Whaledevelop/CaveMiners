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
    public CharacterActionGameEvent startEvent;
    public CharacterActionGameEvent endEvent;

    // Данные экземпляра
    [NonSerialized] protected Rotator rotator;
    [NonSerialized] protected CharacterActionData actionData;

    public virtual void InitInstance(Animator animator, CharacterToolsManager toolsManager, Rotator rotator, CharacterActionData actionData)
    {
        base.InitInstance(animator, toolsManager);
        this.rotator = rotator;
        this.actionData = actionData;
    }

    public override IEnumerator Start()
    {
        yield return base.Start();
        rotator.Rotate(actionData.actionDirection, rotationMode);
        if (startEvent != null)
        {
            startEvent.Raise(actionData);
        }
    }

    public override IEnumerator End()
    {
        yield return base.End();
        if (endEvent != null)
        {
            endEvent.Raise(actionData);
        }
    }
}
