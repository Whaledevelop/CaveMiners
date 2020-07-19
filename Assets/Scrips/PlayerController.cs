using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask blockingLayer;
    public SpriteRenderer sprite;
    public bool flipTopDown = true;
    public bool flipLeftRight = true;

    // Components
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D boxCollider2D;

    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        if (xInput != 0)
        {
            movement.x = xInput;

            if (flipLeftRight)
            {
                if (movement.x > 0 && transform.localScale.x == -1)             // Повернут влево, а нужно повернуть вправо
                    transform.localScale = new Vector2(1, transform.localScale.y);
                else if (movement.x < 0 && transform.localScale.x == 1)         // Повернут вправо, а нужно повернуть влево
                    transform.localScale = new Vector2(-1, transform.localScale.y);
            }

            animator.SetFloat("Speed", Math.Abs(movement.x));
        }
        else if (yInput != 0)
        {
            movement.y = yInput;

            if (flipTopDown)
            {
                if (movement.y > 0 && transform.localScale.y == 1)              // Повернут сверху вниз, а нужно повернуть снизу вверх
                    transform.localScale = new Vector2(transform.localScale.x, -1);
                else if (movement.y < 0 && transform.localScale.y == -1)         // Повернут снизу вверх, а нужно повернуть сверху вниз
                    transform.localScale = new Vector2(transform.localScale.x, 1);
            }
            animator.SetFloat("Speed", Math.Abs(movement.y));
        }
        else
        {
            animator.SetFloat("Speed", 0);            
        }       
    }

    Vector2 movementEndPoint;

    void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {             
            if (!CheckIfMovementBlocked())
            {
                movementEndPoint = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(movementEndPoint);
            }
                
            movement = Vector2.zero;
        }
    }

    public float blockingLayerOffset;

    private bool CheckIfMovementBlocked()
    {
        if (blockingLayer == default)
        {
            return false;
        }            
        else
        {
            if (movement.x != 0)
            {
                float xColliderOffset = movement.x * (boxCollider2D.offset.x + boxCollider2D.size.x / 2);

                float yOffset = boxCollider2D.offset.y + boxCollider2D.size.y / 2;

                bool bottomPointBlock = CheckIfOffsetBlocked(new Vector2(xColliderOffset, -yOffset), movement * 0.9f);
                if (bottomPointBlock)
                    return true;
                else
                {
                    Vector2 offset = new Vector2(xColliderOffset, -yOffset) + movement * (blockingLayerOffset);
                    bool offsetBottomBlock = CheckIfOffsetBlocked(offset, new Vector2(0, -0.9f));
                    if (offsetBottomBlock)
                        return true;
                    else
                        return CheckIfOffsetBlocked(offset, new Vector2(0, 0.9f));
                }                    
            }
            else if (movement.y != 0)
            {
                float yColliderOffset = -(boxCollider2D.offset.y + boxCollider2D.size.y / 2); // И при движении вверх, и вниз, отступ считаем от ног

                float xOffset = boxCollider2D.offset.x + boxCollider2D.size.x / 2;

                bool rightPointBlocked = CheckIfOffsetBlocked(new Vector2(xOffset, yColliderOffset), movement);

                if (rightPointBlocked)
                    return true;
                else
                    return CheckIfOffsetBlocked(new Vector2(-xOffset, yColliderOffset), movement);
            }
            return false;
        }
    }

    private (Vector2, Vector2) gizmosPoints;

    private bool CheckIfOffsetBlocked(Vector2 colliderOffset, Vector2 blockingLayerAxis)
    {
        Vector2 colliderEdgePoint = rb.position + colliderOffset;
        Vector2 blockingLayerEdgePoint = colliderEdgePoint + blockingLayerAxis * blockingLayerOffset;

        gizmosPoints = (blockingLayerEdgePoint, colliderEdgePoint);

        // Не хотим попасть в наш собственный коллайдер
        boxCollider2D.enabled = false;
        // Проверяем, есть ли блокирующий слов на пути движения
        RaycastHit2D hit = Physics2D.Linecast(colliderEdgePoint, blockingLayerEdgePoint, blockingLayer);

        boxCollider2D.enabled = true;

        return hit.transform != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(gizmosPoints.Item1, 0.05f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(gizmosPoints.Item2, 0.05f);
    }
}
