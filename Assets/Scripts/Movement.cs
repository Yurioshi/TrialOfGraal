using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Animator animator;
    CharacterController cc;
    Vector3 velocity;
    Vector3 rootMotion;
    public float dashForce;
    public float gravity;
    float inputX;
    float inputY;
    public static bool isInAir;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        Vector2 input = new Vector2(inputX, inputY);

        //isInAir = !cc.isGrounded;

        LocomotionAnimatons(input);

        if (Input.GetKeyDown(KeyCode.Space) && !MeleeAttack.isAttacking && !Block.isBlocking)
        {
            Dash();
        }
    }
    void FixedUpdate()
    {
        if (isInAir)
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
            cc.Move(velocity * Time.fixedDeltaTime);
            isInAir = !cc.isGrounded;
            rootMotion = Vector3.zero;
        }
        else
        {
            cc.Move(rootMotion);
            rootMotion = Vector3.zero;
        }
    }

    void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    void LocomotionAnimatons(Vector2 input)
    {
        float speed = 0f;

        if (input.x != 0 || input.y != 0)     //Is Walking
        {
            speed = 1f;
            
            if (Input.GetKey(KeyCode.LeftShift))    //Press Shift to Run
            {
                speed = 2f;
            }
        }

        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }
    void Dash()
    {
        if(!isInAir)
        {
            isInAir = true;
            Vector3 jumpDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            if(jumpDirection.magnitude != 0)
            {
                jumpDirection = transform.TransformDirection(jumpDirection);
                jumpDirection.y = .3f;
                velocity = jumpDirection.normalized * dashForce;
            }
        }
    }
}