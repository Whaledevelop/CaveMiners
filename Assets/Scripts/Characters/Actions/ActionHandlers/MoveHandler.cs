using System.Collections;
using UnityEngine;

/// <summary>
/// Обработчик движения персонажа
/// </summary>
public class MoveHandler : CharacterActionHandler
{
    [SerializeField] private CharacterActionState moveState;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float defaultSpeed;
    [SerializeField] private CharacterActionGameEvent endMovementEvent;

    private bool isMoving;

    public override CharacterActionState HandledState => moveState;

    public override IEnumerator Execute(CharacterAction actionData)
    {
        isMoving = true;
        // Начинаем движение. Скорость зависит от базовой скорости и таланта персонажа
        yield return Move(actionData.endPosition, defaultSpeed * actionData.SkillValue);
        isMoving = false;
        endMovementEvent.Raise(actionData);
    }

    public IEnumerator Move(Vector2 moveEndPoint, float speed)
    {
        while(isMoving && Vector2.Distance(rb.position, moveEndPoint) > 0.1)
        {
            Vector2 movement = Vector2.MoveTowards(rb.position, moveEndPoint, speed * Time.fixedDeltaTime);
            rb.MovePosition(movement);
            yield return new WaitForFixedUpdate();
        }          
    }

    public override IEnumerator Cancel()
    {
        isMoving = false;
        yield break;
    }
}
