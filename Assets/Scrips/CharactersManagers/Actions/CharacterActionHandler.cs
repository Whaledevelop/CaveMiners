using UnityEngine;
using System.Collections;

public abstract class CharacterActionHandler : MonoBehaviour
{
    public abstract CharacterActionUnityEvent onActionEndEvent { get; }

    public abstract void OnAction(CharacterActionData actionData);

    public abstract void CancelAction();


}
