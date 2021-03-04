using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector3 inputMovement, movement, lastMoveDir, lastLookedPosition, lookDirection;
    float moveSpeed = 10f;

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
                break;
        }

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
    public virtual void FixedHandleMovement()
    {
        rb.velocity = movement * moveSpeed;
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

}
