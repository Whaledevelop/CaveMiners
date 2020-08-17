using UnityEngine;
using System.Collections;

public abstract class CameraMode : ScriptableObject
{
    public abstract IEnumerator Execute(CameraController cameraController);
}
