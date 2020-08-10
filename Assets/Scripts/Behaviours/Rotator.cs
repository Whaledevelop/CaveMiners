using UnityEngine;

public enum RotationMode
{
    None,
    LeftRight,
    LeftRightDown
}

public class Rotator : MonoBehaviour
{
    [SerializeField] private Transform rotatingTransform;

    public void Rotate(Vector2 direction, RotationMode rotationMode)
    {
        switch (rotationMode)
        {
            case RotationMode.LeftRight:

                RotateLeftRight(direction.x);

                if (rotatingTransform.eulerAngles.z != 0)
                {
                    rotatingTransform.eulerAngles = new Vector3(rotatingTransform.eulerAngles.x, rotatingTransform.eulerAngles.y, 0);
                }
                break;
            case RotationMode.LeftRightDown:

                RotateLeftRight(direction.x);

                if (direction.y != 0)
                {
                    float z = rotatingTransform.localScale.x * direction.y * 90;
                    rotatingTransform.eulerAngles = new Vector3(rotatingTransform.eulerAngles.x, rotatingTransform.eulerAngles.y, z);
                }
                break;
            default: break;
        }         
    }

    private void RotateLeftRight(float xDirection)
    {
        if (xDirection != 0)
        {
            rotatingTransform.localScale = new Vector2(xDirection, rotatingTransform.localScale.y);
        }

    }
}
