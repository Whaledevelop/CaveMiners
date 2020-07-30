using UnityEngine;
using System.Collections;

public interface IActionHandler
{
    void OnStartAction(CharacterActionData actionData);

    void OnEndAction(CharacterActionData actionData);
}

public interface IIteractiveActionHandler : IActionHandler
{
    void OnIteration(CharacterActionData actionData);
    void OnStartAction(CharacterActionData actionData);
}
