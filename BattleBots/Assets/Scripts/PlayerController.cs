using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector3 inputMovement, movement, lastMoveDir, lastLookedPosition, lookDirection;
    bool pressedRight, pressedLeft, releasedRight, releasedLeft, punchedRight, punchedLeft, returningLeft, returningRight = false;
    float moveSpeed = 12f;
    float punchedLeftTimer, punchedRightTimer;
    float inputBuffer = .15f;
    [SerializeField] Transform leftHandTransform, rightHandTransform;
    float punchRange = 2f;
    float punchSpeed = 40f;
    float returnSpeed = 4f;
    SphereCollider leftHandCollider, rightHandCollider;

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
        rb.velocity = movement * moveSpeed;
    }

    void HandleThrowingHands()
    {
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
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(punchRange, 0, .4f), punchSpeed * Time.deltaTime);
            if (rightHandTransform.localPosition.x >= punchRange)
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



    protected virtual void FaceLookDirection()
    {
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
        if (pressedLeft)
        {
            punchedLeftTimer = inputBuffer;
            pressedLeft = false;
        }

        if (punchedLeftTimer > 0)
        {
            if (lookDirection.magnitude != 0)
            {
                transform.right = lookDirection;
            }

            punchedLeft = true;
            punchedLeftTimer = 0;
        }
    }
    void CheckForPunchRight()
    {
        if (pressedRight)
        {
            punchedRightTimer = inputBuffer;
            pressedRight = false;
        }

        if (punchedRightTimer > 0)
        {
            if (lookDirection.magnitude != 0)
            {
                transform.right = lookDirection;
            }

            punchedRight = true;
            punchedRightTimer = 0;
        }
    }
}
