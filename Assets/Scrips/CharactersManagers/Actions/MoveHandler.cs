using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandler : CharacterActionHandler
{    
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float speed;

    [SerializeField] private CharacterActionUnityEvent onMoveEnd;

    private Vector2 moveEndPoint;
    private bool isMoving;

    public override CharacterActionUnityEvent onActionEndEvent => onMoveEnd;


    public void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 movement = Vector2.MoveTowards(rb.position, moveEndPoint, speed * Time.fixedDeltaTime);
            rb.MovePosition(movement);
        }
    }

    public IEnumerator WaitUntilEndPoint(CharacterActionData actionData)
    {
        yield return new WaitUntil(() => Vector2.Distance(rb.position, moveEndPoint) < 0.1);
        onActionEndEvent.Invoke(actionData);
        isMoving = false;
    }

    public override void OnAction(CharacterActionData actionData)
    {        
        if (isMoving)
        {
            StopAllCoroutines();
        }
        moveEndPoint = actionData.endPosition;
        isMoving = true;
        StartCoroutine(WaitUntilEndPoint(actionData));
    }

    public override void CancelAction()
    {
        isMoving = false;
    }
}
