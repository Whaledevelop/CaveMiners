using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "FollowCharacterCameraMode", menuName = "CameraModes/FollowCharacterCameraMode")]
public class FollowCharacterCameraMode : CameraMode
{
    [SerializeField] private CharactersSet charactersSet;
    [SerializeField] private float defaultZoom;

    public override IEnumerator Execute(CameraController cameraController)
    {
        if (charactersSet.ActiveCharacter != null)
        {
            yield return new WaitForEndOfFrame();
            Vector3 activeCharacterPosition = charactersSet.ActiveCharacter.transform.position;
            activeCharacterPosition.z = cameraController.transform.position.z;
            cameraController.transform.position = activeCharacterPosition;
            cameraController.controlledCamera.orthographicSize = defaultZoom;
        }
    }
}
