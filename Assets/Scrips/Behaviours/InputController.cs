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

    public CharacterStateData digData;
    public CharacterStateMachine characterStateMachine;

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            characterStateMachine.SetState(digData);
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
