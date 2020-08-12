using System.Collections;
using UnityEngine;

public class MoveHandler : CharacterActionHandler
{
    [SerializeField] private CharacterActionState moveState;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float defaultSpeed;

    [SerializeField] private CharacterActionGameEvent endMovementEvent;

    [HideInInspector] public float speed;

    private Vector2 moveEndPoint;
    private bool isMoving;

    public override CharacterActionState HandledState => moveState;

    public void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 movement = Vector2.MoveTowards(rb.position, moveEndPoint, speed * Time.fixedDeltaTime);
            rb.MovePosition(movement);
        }
    }

    public override IEnumerator Execute(CharacterAction actionData)
    {
        moveEndPoint = actionData.endPosition;
        speed = defaultSpeed * actionData.stateSkill;
        isMoving = true;
        yield return new WaitUntil(() => Vector2.Distance(rb.position, moveEndPoint) < 0.1);
        isMoving = false;
        endMovementEvent.Raise(actionData);
    }

    public override IEnumerator Cancel()
    {
        isMoving = false;
        moveEndPoint = default;
        speed = defaultSpeed;
        yield break;
    }
}
