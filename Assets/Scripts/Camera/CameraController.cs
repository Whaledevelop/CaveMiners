﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Скрипт, регулирующий поведение основной камеры при помощи режимов камеры
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    #region Zoom

    [SerializeField] private Range zoomRange;
    [Range(1, 10)]
    [SerializeField] private int scrollSpeed = 5;

    #endregion

    [SerializeField] private CameraMode[] cameraModes;

    [HideInInspector] public Camera controlledCamera;
    [HideInInspector] public LevelSettings levelSettings;

    private CameraMode activeCameraMode;
    private int activeCameraModeIndex;

    private IEnumerator activeCameraEnumerator;
    public void Init(LevelSettings levelSettings)
    {
        this.levelSettings = levelSettings;
        controlledCamera = GetComponent<Camera>();
        SetCameraMode(cameraModes[0]);
    }

    // Метод для UnityEvent, когда при нажатии на клавишу меняется режим камеры
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

    /// <summary>
    /// При изменении режима камеры контроль переходит к режиму, и он регулирует поведение
    /// </summary>
    /// <param name="cameraMode"></param>
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

    // Скролл пока одинаковый для всех режимов камеры, но можно в дальнейшем перенести это поведение в моды
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

    #region Camera modes input

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

    #endregion
}
