using UnityEngine;
using System.Collections;

/// <summary>
/// CharacterActionHandler реализует определенное действие "на местности" - это MonoBehaviour,
/// в котором могут быть конкретные ссылки (в отличие от ScriptableObject стейта)
/// </summary>
public abstract class CharacterActionHandler : MonoBehaviour
{
    public abstract IEnumerator Execute(CharacterAction actionData);

    public abstract IEnumerator Cancel();

    public abstract CharacterActionState HandledState {get;}
}
