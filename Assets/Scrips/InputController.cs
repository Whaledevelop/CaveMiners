using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Animator))]
public class InputController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool flipTopDown = true;
    public bool flipLeftRight = true;

    public MovingObject movingObject;

    private Animator animator;
    private Vector2 movement;

    void Start()
    {
        animator = GetComponent<Animator>();
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
                // Поворачиваем, если повернут в обратную сторону от движения
                if (movement.x != transform.localScale.x)
                    transform.localScale = new Vector2(movement.x, transform.localScale.y);
            }

            animator.SetFloat("Speed", Math.Abs(movement.x));
        }
        else if (yInput != 0)
        {
            movement.y = yInput;

            if (flipTopDown)
            {
                // Переворачиваем, если повернут в обратную сторону от движения
                if (movement.y != -transform.localScale.y)
                    transform.localScale = new Vector2(transform.localScale.x, -movement.y);
            }
            animator.SetFloat("Speed", Math.Abs(movement.y));
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }


    void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            movingObject.Move(movement * moveSpeed * Time.fixedDeltaTime);

            movement = Vector2.zero;
        }
    }
}
