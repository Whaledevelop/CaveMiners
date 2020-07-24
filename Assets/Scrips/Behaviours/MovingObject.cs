using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MovingObject : MonoBehaviour
{    
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float speed;    
    
    public UnityEvent moveEvent;

    private Vector2 moveEndPoint;
    private bool isMoving;

    public void Move(Vector2 moveEndPoint)
    {
        this.moveEndPoint = moveEndPoint;

        isMoving = true;

        StartCoroutine(WaitUntilEndPoint());
    }

    public void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 movement = Vector2.MoveTowards(rb.position, moveEndPoint, speed * Time.fixedDeltaTime);
            rb.MovePosition(movement);
        }
    }

    public IEnumerator WaitUntilEndPoint()
    {
        yield return new WaitUntil(() => Vector2.Distance(rb.position, moveEndPoint) < 0.1);
        moveEvent.Invoke();
    }
}
