
using UnityEngine;
using UnityEngine.InputSystem;

public interface IHandleLookInput
{
    void OnLookInput(Vector2 lookInput);
}

public interface IHandleExecuteInput
{
    void OnExecuteInput(InputActionPhase inputActionPhase);
}
