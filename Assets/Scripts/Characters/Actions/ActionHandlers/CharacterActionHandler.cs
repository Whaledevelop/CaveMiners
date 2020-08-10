using UnityEngine;
using System.Collections;

public abstract class CharacterActionHandler : MonoBehaviour
{
    public abstract IEnumerator Execute(CharacterAction actionData);

    public abstract IEnumerator Cancel();

    public abstract CharacterActionState HandledState {get;}
}
