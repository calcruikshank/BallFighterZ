﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector3 inputMovement, movement, lastMoveDir, lastLookedPosition, lookDirection, oppositeForce;
    bool pressedRight, pressedLeft, releasedRight, releasedLeft, punchedRight, punchedLeft, returningLeft, returningRight = false;
    float moveSpeed = 16f;
    float punchedLeftTimer, punchedRightTimer, currentPercentage, brakeSpeed;
    float inputBuffer = .15f;
    [SerializeField] Transform leftHandTransform, rightHandTransform;
    float punchRange = 3f;
    float punchRangeRight = 4f;
    float punchSpeed = 40f;
    float returnSpeed = 10f;
    SphereCollider leftHandCollider, rightHandCollider;
    [SerializeField] Animator animator;

    public State state;
    public enum State
    {
        Normal,
        Knockback,
        Diving,
        Grabbed,
        Grabbing,
        Stunned,
        Dashing,
        PowerShieldStunned,
        PowerShielding,
        PowerDashing,
        ShockGrabbed,
        FireGrabbed,
        UltimateState,
        TakingUltimate
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        state = State.Normal;
        leftHandCollider = leftHandTransform.GetComponent<SphereCollider>();
        rightHandCollider = rightHandTransform.GetComponent<SphereCollider>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    protected virtual void Update()
    {
        switch (state)
        {
            case State.Normal:
                HandleMovement();
                HandleThrowingHands();
                break;
            case State.Knockback:
                HandleKnockback();
                HandleThrowingHands();
                break;
        }

        CheckForInputs();
        FaceLookDirection();
    }

    protected virtual void FixedUpdate()
    {

        switch (state)
        {
            case State.Normal:
                FixedHandleMovement();
                break;
        }
    }


    void HandleMovement()
    {
        movement.x = inputMovement.x;
        movement.z = inputMovement.y;
        if (movement.x != 0 || movement.y != 0)
        {
            lastMoveDir = movement;
        }
    }
    protected virtual void FixedHandleMovement()
    {
        Vector3 newVelocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, movement.z * moveSpeed);
        rb.velocity = newVelocity;
    }

    void HandleThrowingHands()
    {
        if (animator != null)
        {
            animator.SetBool("PunchRightAnimation", (punchedRight));
            if (returningRight == false && punchedRight == false)
            {
                animator.SetBool("RightHasReturned", (true));
            }
            else
            {
                animator.SetBool("RightHasReturned", (false));
            }
        }
        if (punchedLeft && returningLeft == false)
        {
            punchedLeftTimer = 0;
            leftHandCollider.enabled = true;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector3(punchRange, 0, -.4f), punchSpeed * Time.deltaTime);
            if (leftHandTransform.localPosition.x >= punchRange)
            {
                returningLeft = true;
            }
        }
        if (returningLeft)
        {
            punchedLeft = false;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector3(0, 0, 0), returnSpeed * Time.deltaTime);

            if (leftHandTransform.localPosition.x <= 1f)
            {
                leftHandCollider.enabled = false;
            }
            if (leftHandTransform.localPosition.x <= 0f)
            {
                returningLeft = false;
            }
        }



        if (punchedRight && returningRight == false)
        {
            punchedRightTimer = 0;
            rightHandCollider.enabled = true;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(punchRangeRight, 0, .4f), punchSpeed * Time.deltaTime);
            if (rightHandTransform.localPosition.x >= punchRangeRight)
            {
                returningRight = true;
            }
        }
        if (returningRight)
        {
            punchedRight = false;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(0, 0, 0), returnSpeed * Time.deltaTime);

            if (rightHandTransform.localPosition.x <= 1f)
            {
                rightHandCollider.enabled = false;
            }
            if (rightHandTransform.localPosition.x <= 0f)
            {
                returningRight = false;
            }
        }

    }

    public void Knockback(float damage, Vector3 direction)
    {
        currentPercentage += damage;
        brakeSpeed = 20f;
        // Debug.Log(damage + " damage");
        //Vector2 direction = new Vector2(rb.position.x - handLocation.x, rb.position.y - handLocation.y); //distance between explosion position and rigidbody(bluePlayer)
        //direction = direction.normalized;
        float knockbackValue = (14 * ((currentPercentage + damage) * (damage / 2)) / 150) + 7; //knockback that scales
        rb.velocity = (direction * knockbackValue);
        state = State.Knockback;
    }
    void HandleKnockback()
    {
        if (rb.velocity.magnitude <= 5)
        {
            rb.velocity = new Vector2(0, 0);
            state = State.Normal;
        }
        if (rb.velocity.magnitude > 0)
        {
            oppositeForce = -rb.velocity;
            brakeSpeed = brakeSpeed + (100f * Time.deltaTime);
            rb.AddForce(oppositeForce * Time.deltaTime * brakeSpeed);
            rb.AddForce(movement * .05f); //DI
        }
    }








    void OnMove(InputValue value)
    {
        inputMovement = value.Get<Vector2>();
        lookDirection = value.Get<Vector2>();
    }

    private void OnButtonSouth()
    {
        Debug.Log("pressed a");
    }
    void OnPunchRight()
    {
        pressedRight = true;
        releasedRight = false;
    }
    void OnPunchLeft()
    {
        pressedLeft = true;
        releasedLeft = false;
    }
    void OnReleaseRight()
    {
        pressedRight = false;
        releasedRight = true;
    }
    void OnReleaseLeft()
    {
        pressedLeft = false;
        releasedLeft = true;
    }



    protected virtual void FaceLookDirection()
    {
        if (punchedLeft || punchedRight || leftHandTransform.localPosition.x > .1f && returningLeft || rightHandTransform.localPosition.x > .1f && returningRight) return;
        Vector3 lookTowards = new Vector3(lookDirection.x, 0, lookDirection.y);
        if (lookTowards.x != 0 || lookTowards.y != 0)
        {
            lastLookedPosition = lookTowards;
        }

        Look();
    }

    protected virtual void Look()
    {
        transform.right = lastLookedPosition;
    }

    void CheckForInputs()
    {
        CheckForPunchRight();
        CheckForPunchLeft();
    }

    void CheckForPunchLeft()
    {
        if (releasedLeft)
        {
            punchedLeftTimer -= Time.deltaTime;
        }
        if (pressedLeft)
        {
            punchedLeftTimer = inputBuffer;
            pressedLeft = false;
        }

        if (returningLeft) return;

        if (punchedLeftTimer > 0)
        {
            if (lookDirection.magnitude != 0)
            {
                Vector3 lookTowards = new Vector3(lookDirection.x, 0, lookDirection.y);
                transform.right = lookTowards;
            }

            punchedLeft = true;
            punchedLeftTimer = 0;
        }
    }
    void CheckForPunchRight()
    {
        if (releasedRight)
        {
            punchedRightTimer -= Time.deltaTime;
        }
        if (pressedRight)
        {
            punchedRightTimer = inputBuffer;
            pressedRight = false;
        }

        if (returningRight) return;

        if (punchedRightTimer > 0)
        {
            if (lookDirection.magnitude != 0)
            {
                Vector3 lookTowards = new Vector3(lookDirection.x, 0, lookDirection.y);
                transform.right = lookTowards;
            }

            punchedRight = true;
            punchedRightTimer = 0;
        }
    }
}
