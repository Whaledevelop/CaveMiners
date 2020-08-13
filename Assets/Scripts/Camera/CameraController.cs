using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Range zoomRange;
    [Range(1, 10)]
    [SerializeField] private int scrollSpeed = 5;
    [SerializeField] private CameraMode[] cameraModes;

    [HideInInspector] public Camera controlledCamera;
    [HideInInspector] public LevelSettings levelSettings;

    private CameraMode activeCameraMode;
    private int activeCameraModeIndex;
    private Range xCameraMoveRange;

    private IEnumerator activeCameraEnumerator;
    public void Init(LevelSettings levelSettings)
    {
        this.levelSettings = levelSettings;
        controlledCamera = GetComponent<Camera>();
        SetCameraMode(cameraModes[0]);
    }

    public void SetNextCameraMode(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            activeCameraModeIndex++;
            if (activeCameraModeIndex >= cameraModes.Length)
                activeCameraModeIndex = cameraModes.Length - activeCameraModeIndex;
            SetCameraMode(cameraModes[activeCameraModeIndex]);
        }
    }

    public void SetCameraMode(CameraMode cameraMode)
    {
        if (activeCameraMode != cameraMode)
        {
            if (activeCameraEnumerator != null)
                StopCoroutine(activeCameraEnumerator);
            activeCameraEnumerator = cameraMode.Execute(this);
            StartCoroutine(activeCameraEnumerator);
            activeCameraMode = cameraMode;
        }
    }

    public void OnScrollInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Vector2 scrollInput = callbackContext.ReadValue<Vector2>().normalized;
            float newZoom = controlledCamera.orthographicSize + -scrollInput.y / 10 * scrollSpeed;
            if (zoomRange.IsInRange(newZoom))
                controlledCamera.orthographicSize = newZoom;
        }           
    }

    public void OnChooseInput(InputAction.CallbackContext callbackContext)
    {
        if (activeCameraMode is IHandleChooseInput)
            (activeCameraMode as IHandleChooseInput).OnChooseInput(callbackContext);
    }

    public void OnLookInput(InputAction.CallbackContext callbackContext)
    {
        if (activeCameraMode is IHandleLookInput)
            (activeCameraMode as IHandleLookInput).OnLookInput(callbackContext);
    }

    public void OnExecuteInput(InputAction.CallbackContext callbackContext)
    {
        if (activeCameraMode is IHandleExecuteInput)
            (activeCameraMode as IHandleExecuteInput).OnExecuteInput(callbackContext);
    }
}
