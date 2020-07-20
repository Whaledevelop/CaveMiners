using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class MovingObject : MonoBehaviour
{    
    [SerializeField] private LayerMask blockingLayer;
    [SerializeField] private float blockingLayerOffset;

    // Компоненты
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;

    // Для Гизмо
    private Vector2 movementEndPoint;
    private Vector2 movementOutsidePoint;
    private Vector2 colliderEdgePoint;
    private Vector2 blockingLayerEdgePoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }    

    public void Move(float x, float y) => Move(new Vector2(x, y));

    public void Move(Vector2 moveDistance)
    {
        // Проверяем, не заблокировано ли движение
        if (!CheckIfMovementBlocked(moveDistance))
        {
            movementEndPoint = rb.position + moveDistance;
            rb.MovePosition(movementEndPoint);
        }
    }

    /// <summary>
    /// Проверка, заблокировано ли движение. Из краев коллайдера посылаются лучи в сторону движения,
    /// если там блокирующий слой, то false</summary>
    /// <param name="moveDistance">Предполагаемое передвижение</param>
    private bool CheckIfMovementBlocked(Vector2 moveDistance)
    {
        if (blockingLayer == default)  // Если не указан блокирующий слой, то проверка не имеет смысла
            return false;         
        else
        {
            Vector2 normalizedMovement = moveDistance.normalized; // Нужно для определения знака направления движения

            if (moveDistance.x != 0)
            {               
                // Расстояние между центром и краем коллайдера - в сторону движения
                float xColliderOffset = normalizedMovement.x * (boxCollider2D.offset.x + boxCollider2D.size.x / 2);

#if UNITY_EDITOR
                movementOutsidePoint = rb.position + new Vector2(xColliderOffset + moveDistance.x, 0);
#endif
                // Расстояние между центром и краем коллайдера по оси y
                float yColliderOffset = boxCollider2D.offset.y + boxCollider2D.size.y / 2;

                // Сначала проверяем нижний угол (нижний, т.к. блокировка определяется по уровню ног) по направлению к движению
                // 0.9 - чтобы можно было начать движение влево/вправо, если заблокировано вверх/вниз
                bool bottomPointBlock = CheckIfOffsetBlocked(new Vector2(xColliderOffset, -yColliderOffset), normalizedMovement * 0.9f);
                if (bottomPointBlock)
                    return true;
                else // Для движения у углов блокирующего слоя. 
                {                    
                    // Проверяем, нет ли блокирующего слоя под/над точкой, к которой мы движемся по x. 
                    // Сначала под (-0.9), потом над (0.9)
                    Vector2 offset = new Vector2(xColliderOffset, -yColliderOffset) + normalizedMovement * (blockingLayerOffset);
                    bool offsetBottomBlock = CheckIfOffsetBlocked(offset, new Vector2(0, -0.9f));
                    if (offsetBottomBlock)
                        return true;
                    else
                        return CheckIfOffsetBlocked(offset, new Vector2(0, 0.9f));
                }                    
            }
            else if (moveDistance.y != 0)
            {
                // И при движении вверх, и вниз, отступ считаем от ног
                float yColliderOffset = (-transform.localScale.y) * (boxCollider2D.offset.y + boxCollider2D.size.y / 2);
#if UNITY_EDITOR
                movementOutsidePoint = rb.position + new Vector2(0, normalizedMovement.y * (boxCollider2D.offset.y + boxCollider2D.size.y / 2) + moveDistance.y);
#endif
                // Расстояние между центром и краем коллайдера по оси x
                float xColliderOffset = boxCollider2D.offset.x + boxCollider2D.size.x / 2;

                // Проверяем правую точку, ближайшую по направлению к движению
                bool rightPointBlocked = CheckIfOffsetBlocked(new Vector2(xColliderOffset, yColliderOffset), normalizedMovement);

                if (rightPointBlocked)
                    return true;
                else
                    return CheckIfOffsetBlocked(new Vector2(-xColliderOffset, yColliderOffset), normalizedMovement); // Проверяем левую точку
            }
            return false;
        }
    }

    /// <summary>
    /// Проверка, заблокировано ли движение в указанном направлении пусканием луча
    /// </summary>
    /// <param name="colliderOffset">Отступ от середины коллайдера, для определения крайней точки коллайдера - начальной точки луча</param>
    /// <param name="blockingLayerAxis">Ось, по которой нужно проверить наличие блокирующего слоя</param>
    /// <returns></returns>
    private bool CheckIfOffsetBlocked(Vector2 colliderOffset, Vector2 blockingLayerAxis)
    {
        // Крайняя точка коллайдера - начальная точки луча
        colliderEdgePoint = rb.position + colliderOffset;
        // Крайняя точка, к которой допустимо движение, с учетом отступа
        blockingLayerEdgePoint = colliderEdgePoint + blockingLayerAxis * blockingLayerOffset;

        // Не хотим попасть в наш собственный коллайдер
        boxCollider2D.enabled = false;
        // Проверяем, есть ли блокирующий слой между крайней точки коллайдера и крайней допустимой точкой рядом с блокирующим слоем
        RaycastHit2D hit = Physics2D.Linecast(colliderEdgePoint, blockingLayerEdgePoint, blockingLayer);

        // Заново включаем коллайдер
        boxCollider2D.enabled = true;

        return hit.transform != null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(colliderEdgePoint, 0.05f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(blockingLayerEdgePoint, 0.05f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(movementEndPoint, 0.05f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(movementOutsidePoint, 0.05f);
    }
#endif
}
