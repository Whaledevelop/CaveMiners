using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public enum CameraMode
    {
        MapObservation,
        FollowCharacter
    }

    [SerializeField] private CharacterTasksManagersSet charactersSet;

    [SerializeField] private Range zoomRange;

    [SerializeField] private float cameraZOffset;

    private CameraMode cameraMode;
    private Camera controlledCamera;
    private Vector3 mapObservationPosition;
    private float mapObservationZoom;

    private float zoomToCharacter;
    private float ZoomToCharacter => zoomToCharacter != default ? zoomToCharacter : zoomRange.Average;

    public void Start()
    {
        controlledCamera = GetComponent<Camera>();
        mapObservationPosition = transform.position;
        mapObservationZoom = controlledCamera.orthographicSize;
    }

    public void Update()
    {
        if (cameraMode == CameraMode.FollowCharacter)
        {
            SetFollowCharacterPosition();
        }
    }

    public void OnSwitchCameraMode()
    {
        SetCameraMode(cameraMode == CameraMode.MapObservation ? CameraMode.FollowCharacter : CameraMode.MapObservation);
    }

    public void SetCameraMode(CameraMode cameraMode)
    {
        this.cameraMode = cameraMode;
        switch(cameraMode)
        {
            case CameraMode.MapObservation:

                transform.position = mapObservationPosition;
                controlledCamera.orthographicSize = mapObservationZoom;

                break;

            case CameraMode.FollowCharacter:
                
                SetFollowCharacterPosition();
                controlledCamera.orthographicSize = ZoomToCharacter;

                break;
        }
    }



    private void SetFollowCharacterPosition()
    {
        if (charactersSet.ActiveCharacter != null)
        {
            Vector3 activeCharacterPosition = charactersSet.ActiveCharacter.transform.position;
            activeCharacterPosition.z = cameraZOffset;
            transform.position = activeCharacterPosition;
        }
    }
}
