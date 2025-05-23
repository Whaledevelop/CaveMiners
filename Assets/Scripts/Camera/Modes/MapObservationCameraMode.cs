﻿using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System;

/// <summary>
/// Режим камеры, в котором можно передвигать камеру в пределах уровня зажав левую кнопку мыши
/// </summary>
[CreateAssetMenu(fileName = "MapObservationCameraMode", menuName = "CameraModes/MapObservationCameraMode")]
public class MapObservationCameraMode : CameraMode, IHandleLookInput, IHandleExecuteInput
{
    [SerializeField] private float speed = 1;
    [SerializeField] private bool isInversed = true;
    [SerializeField] private float defaultZoom;

    private bool isMovingCamera;
    private Transform cameraTransform;

    private RangeInt xMovementRange;
    private RangeInt yMovementRange;

    public override IEnumerator Execute(CameraController cameraController)
    {
        cameraTransform = cameraController.transform;
        cameraController.controlledCamera.orthographicSize = defaultZoom;
        xMovementRange = cameraController.levelSettings.XLevelSizeRange;
        yMovementRange = cameraController.levelSettings.YLevelSizeRange;
        yield break;
    }

    public void OnExecuteInput(InputActionPhase inputActionPhase)
    {
        if (inputActionPhase == InputActionPhase.Started)
        {
            isMovingCamera = true;
        }
        else if (inputActionPhase == InputActionPhase.Canceled)
        {
            isMovingCamera = false;
        }
    }

    public void OnLookInput(Vector2 inputVector)
    {
        if (isMovingCamera)
        {
            Vector2 movement = speed * (isInversed ? -inputVector : inputVector) / 100;
            bool isXInRange = xMovementRange.IsInRange(cameraTransform.position.x + movement.x);
            bool isYInRange = yMovementRange.IsInRange(cameraTransform.position.y + movement.y);
            if (isXInRange && isYInRange)
            {
                cameraTransform.Translate(movement);
            }
        }
    }
}
