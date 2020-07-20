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

    // Временно тут
    public Rotator rotator;

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
                rotator.Rotate(movement); // Поворачиваем, если повернут в обратную сторону от движения
            }

            animator.SetFloat("Speed", Math.Abs(movement.x));
        }
        else if (yInput != 0)
        {
            movement.y = yInput;

            if (flipTopDown)
            {
                rotator.Rotate(movement); // Поворачиваем, если повернут в обратную сторону от движения
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
