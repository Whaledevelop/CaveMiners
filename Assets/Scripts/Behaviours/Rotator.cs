using UnityEngine;

public enum RotationMode
{
    None,
    LeftRight,
    LeftRightDown
}

public class Rotator : MonoBehaviour
{    
    public void Rotate(Vector2 direction, RotationMode rotationMode)
    {
        switch (rotationMode)
        {
            case RotationMode.LeftRight:
                RotateLeftRight(direction.x);
                break;
            case RotationMode.LeftRightDown:
                RotateLeftRight(direction.x);
                if (direction.y != 0)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.localScale.x * direction.y * 90);
                }
                break;
            default: break;
        }         
    }

    private void RotateLeftRight(float xDirection)
    {
        if (xDirection != 0)
        {
            transform.localScale = new Vector2(xDirection, transform.localScale.y);
            if (transform.eulerAngles.z != 0)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            }
        }
    }
}
