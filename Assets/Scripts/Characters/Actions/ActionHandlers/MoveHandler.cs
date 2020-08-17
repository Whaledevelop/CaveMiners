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
    [Header("Насколько влияет скилл на увеличение скорости")]
    [Tooltip("0 - не влияет, 1 - скорость равно стандартной на скилл")]
    [Range(0, 1)]
    [SerializeField] private float skillWeight = 0.5f;
    [SerializeField] private CharacterActionGameEvent endMovementEvent;

    private bool isMoving;

    public override CharacterActionState HandledState => moveState;

    public override IEnumerator Execute(CharacterAction actionData)
    {
        isMoving = true;
        float speed = defaultSpeed + defaultSpeed * (actionData.SkillValue - 1) * skillWeight;
        // Начинаем движение. Скорость зависит от базовой скорости и таланта персонажа
        yield return Move(actionData.endPosition, speed);
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
