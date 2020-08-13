
using UnityEngine.InputSystem;

public interface IHandleLookInput
{
    void OnLookInput(InputAction.CallbackContext callbackContext);
}

public interface IHandleChooseInput
{
    void OnChooseInput(InputAction.CallbackContext callbackContext);
}

public interface IHandleExecuteInput
{
    void OnExecuteInput(InputAction.CallbackContext callbackContext);
}

public interface IHandleScrollInput
{
    void OnScrollInput(InputAction.CallbackContext callbackContext);
}
