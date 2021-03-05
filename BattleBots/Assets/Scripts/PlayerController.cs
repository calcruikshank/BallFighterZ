using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector3 inputMovement, movement, lastMoveDir, lastLookedPosition, lookDirection, oppositeForce, powerDashTowards;
    bool pressedRight, pressedLeft, releasedRight, releasedLeft, returningLeft, returningRight, pressedShield, releasedShield = false;
    public bool punchedRight, punchedLeft, shielding, isParrying = false;
    float parryTimerThreshold = .15f;
    float moveSpeed, moveSpeedSetter = 18f;
    float punchedLeftTimer, punchedRightTimer, currentPercentage, brakeSpeed, canShieldAgainTimer, parryTimer, parryStunnedTimer, isParryingTimer, powerDashSpeed;
    float inputBuffer = .15f;
    [SerializeField] Transform leftHandTransform, rightHandTransform;
    [SerializeField] GameObject splatterPrefab;
    float punchRange = 3f;
    float punchRangeRight = 4f;
    float punchSpeed = 40f;
    float returnSpeed = 15f;
    int stocks = 4;

    CameraShake cameraShake;

    SphereCollider leftHandCollider, rightHandCollider;
    [SerializeField] Animator animator;
    [SerializeField] Transform shield;
    [SerializeField] GameObject arrow;

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
        ShockGrabbed,
        FireGrabbed,
        UltimateState,
        TakingUltimate,
        ParryState,
        PowerDashing,
        ParryStunned
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        state = State.Normal;
        leftHandCollider = leftHandTransform.GetComponent<SphereCollider>();
        rightHandCollider = rightHandTransform.GetComponent<SphereCollider>();
        cameraShake = FindObjectOfType<CameraShake>();
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
                HandleShielding();
                break;
            case State.Knockback:
                HandleKnockback();
                HandleThrowingHands();
                break;
            case State.ParryState:
                HandleParry();
                HandleShielding();
                break;
            case State.PowerDashing:
                HandlePowerDashing();
                HandleShielding();
                HandleThrowingHands();
                break;
            case State.ParryStunned:
                HandleShielding();
                HandleParryStunned();
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
            case State.PowerDashing:
                FixedHandlePowerDashing();
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

        if (punchedLeft || punchedRight || returningLeft || returningRight)
        {
            moveSpeed = moveSpeedSetter - 8f;
        }
        if (!punchedLeft && !punchedRight && !returningLeft && !returningRight)
        {
            moveSpeed = moveSpeedSetter;
        }
        if (shielding)
        {
            moveSpeed = 0;
        }

    }

    public void Knockback(float damage, Vector3 direction)
    {
        currentPercentage += damage;
        brakeSpeed = 20f;
        // Debug.Log(damage + " damage");
        //Vector2 direction = new Vector2(rb.position.x - handLocation.x, rb.position.y - handLocation.y); //distance between explosion position and rigidbody(bluePlayer)
        //direction = direction.normalized;
        float knockbackValue = (14 * ((currentPercentage + damage) * (damage / 2)) / 150) + 14; //knockback that scales
        rb.velocity = (direction * knockbackValue);
        HitImpact(direction);
        state = State.Knockback;
    }
    void HandleKnockback()
    {
        if (rb.velocity.magnitude <= 8)
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

        Vector3 lookTowards = new Vector3(oppositeForce.x, 0, oppositeForce.z);
        transform.right = lookTowards;
    }

    void HandleShielding()
    {
        if (shielding)
        {
            parryTimer += Time.deltaTime;
            if (parryTimer <= parryTimerThreshold)
            {
                isParrying = true;
            }
            if (parryTimer > parryTimerThreshold)
            {
                isParrying = false;
            }
            shield.gameObject.SetActive(true);
        }
        if (!shielding)
        {
            isParrying = false;
            shield.gameObject.SetActive(false);
        }
    }
    public void Parry()
    {
        if (state == State.ParryState) return;
        rb.velocity = Vector3.zero;
        isParryingTimer = 0;
        state = State.ParryState;
    }
    void HandleParry()
    {
        Time.timeScale = .2f;
        isParryingTimer += Time.deltaTime;

        if (inputMovement.magnitude > .8f)
        {
            arrow.SetActive(true);
        }
        if (inputMovement.magnitude <= .8f)
        {
            arrow.SetActive(false);
        }
        if (isParryingTimer > .2f && inputMovement.magnitude > .8f)
        {
            shielding = false;
            Time.timeScale = 1;

            arrow.SetActive(false);
            PowerDash(inputMovement);
            return;
        }
        if (isParryingTimer > .2f && inputMovement.magnitude <= .8f)
        {
            Time.timeScale = 1;

            arrow.SetActive(false);
            state = State.Normal;
        }
        if (inputMovement.magnitude > .8f && releasedShield)
        {

            Time.timeScale = 1;
            PowerDash(inputMovement);

            arrow.SetActive(false);
            return;
        }
        if (releasedShield)
        {

            Time.timeScale = 1;
            arrow.SetActive(false);
            state = State.Normal;
        }

        if (punchedLeft || punchedRight)
        {
            Time.timeScale = 1;
            shielding = false;
            arrow.SetActive(false);
            PowerDash(inputMovement);
            return;
        }
    }

    void PowerDash(Vector3 powerDashDirection)
    {
        shielding = false;
        canShieldAgainTimer = 0f;
        powerDashSpeed = 80f;
        powerDashTowards = new Vector3(powerDashDirection.normalized.x, rb.velocity.y, powerDashDirection.normalized.y);
        state = State.PowerDashing;
    }

    void HandlePowerDashing()
    {
        Time.timeScale = 1;
        float powerDashSpeedMulti = 6f;
        powerDashSpeed -= powerDashSpeed * powerDashSpeedMulti * Time.deltaTime;
        
        float powerDashMinSpeed = 10f;
        if (powerDashSpeed < powerDashMinSpeed)
        {
            state = State.Normal;
        }
    }
    void FixedHandlePowerDashing()
    {
        rb.velocity = powerDashTowards * powerDashSpeed;
    }

    public void ParryStun()
    {
        parryStunnedTimer = 0f;
        state = State.ParryStunned;
    }

    void HandleParryStunned()
    {
        rb.velocity = Vector3.zero;
        if (punchedRight)
        {
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(punchRange, 0, .4f), punchSpeed * Time.deltaTime);
        }
        if (punchedLeft)
        {
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector3(punchRange, 0, -.4f), punchSpeed * Time.deltaTime);
        }
        rightHandCollider.enabled = false;
        leftHandCollider.enabled = false;
        parryStunnedTimer += Time.deltaTime / Time.timeScale;
        if (parryStunnedTimer >= .5f)
        {
            returningLeft = true;
            returningRight = true;
            punchedRight = false;
            punchedLeft = false;
            state = State.Normal;
        }
    }

    public void LoseStock()
    {
        stocks--;
        if (stocks >= 1)
        {
            Respawn();
        }
        else
        {
            //FinishGame(this); //pass it player controller to see who lost
        }
    }
    void Respawn()
    {
        state = State.Normal;
        transform.position = Vector3.zero;
        currentPercentage = 0f;
    }
    public void HitImpact(Vector3 impactDirection)
    {
        Instantiate(splatterPrefab, transform.position, transform.rotation);
        StartCoroutine(cameraShake.Shake(.03f, .3f));
        StartCoroutine(FreezeFrames(.075f));
    }
    private IEnumerator FreezeFrames(float freezeTime)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(freezeTime);
        Time.timeScale = 1f;
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
    void OnShield()
    {
        pressedShield = true;
        releasedShield = false;
    }
    void OnReleaseShield()
    {
        releasedShield = true;
        pressedShield = false;
        
    }



    protected virtual void FaceLookDirection()
    {
        if (punchedLeft || punchedRight || leftHandTransform.localPosition.x > .1f && returningLeft || rightHandTransform.localPosition.x > .1f && returningRight) return;
        if (state == State.Knockback) return;
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
        CheckForShield();
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
        if (shielding) return;
        if (state == State.Knockback) return;

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
        if (shielding) return;
        if (state == State.Knockback) return;

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

    void CheckForShield()
    {
        if (!shielding)
        {
            canShieldAgainTimer -= Time.deltaTime;
        }
        if (releasedShield)
        {
            if (shielding)
            {
                canShieldAgainTimer = inputBuffer;
            }


            
            shielding = false;
            
        }
        if (punchedRight && punchedLeft || returningLeft && returningRight || punchedRight && returningLeft || punchedLeft && returningRight)
        {
            shielding = false;
            return;
        }
        
        if (pressedShield)
        {
            pressedShield = false;
            if (canShieldAgainTimer > 0f) return;
            parryTimer = 0;
            shielding = true;
        }
    }
}
