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
            movementEndPoint = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
            if (!CheckIfMovementBlocked(movementEndPoint))
                rb.MovePosition(movementEndPoint);
            movement = Vector2.zero;
        }
    }

    private bool CheckIfMovementBlocked(Vector2 movementEndPoint)
    {
        if (blockingLayer == default)
            return false;
        else
        {

            // Не хотим попасть в наш собственный коллайдер
            boxCollider2D.enabled = false;

            // Луч пускаем не из rb.position, не из центра, а ближайшего к стороне движения края коллайдера
            
            float colliderXOffset = movement.x != 0 ? (movement.x > 0 ? boxCollider2D.size.x : -boxCollider2D.size.x) : 0;
            float colliderYOffset = movement.y != 0 ? -boxCollider2D.size.y : 0; // И при движении вверх, и вниз, отступ считаем от ног
            Vector2 colliderOffset = new Vector2(colliderXOffset, colliderYOffset);

            Debug.Log("rb.position : " + rb.position + ", offset : " + colliderOffset + ", with offset : " + (rb.position + colliderOffset));
            // Проверяем, есть ли блокирующий слов на пути движения
            RaycastHit2D hit = Physics2D.Linecast(rb.position + colliderOffset, movementEndPoint, blockingLayer);


            boxCollider2D.enabled = true;

            return hit.transform != null;
        }
    }
}
