using UnityEngine;
using System.Collections;

public interface IActionHandler
{
    void OnStartAction(CharacterActionData actionData);
}

public interface IIteractiveActionHandler : IActionHandler
{
    void OnIteration(CharacterActionData actionData);
}
