using UnityEngine;


public class MovingObject : MonoBehaviour
{    
    [SerializeField] private Rigidbody2D rb;

    public void Move(ParamsObject digParams)
    {
        Vector2 moveEndPoint  = (Vector2)digParams.paramsArray[0];
        rb.MovePosition(moveEndPoint);
    }
}
