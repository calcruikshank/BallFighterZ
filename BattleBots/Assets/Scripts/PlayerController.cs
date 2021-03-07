using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    protected Rigidbody rb;
    protected Vector3 inputMovement, movement, lastMoveDir, lastLookedPosition, lookDirection, oppositeForce, powerDashTowards;
    protected bool pressedRight, pressedLeft, pressedShield, releasedShield, pressedDash, releasedDash, airDodged, releasedAirDodged = false;
    public bool punchedRight, punchedLeft, shielding, isParrying, returningLeft, returningRight, releasedLeft, releasedRight, isDashing = false;
    float parryTimerThreshold = .15f;
    [SerializeField] protected float moveSpeed, moveSpeedSetter = 18f;
    protected float punchedLeftTimer, punchedRightTimer, currentPercentage, brakeSpeed, canShieldAgainTimer, parryTimer, parryStunnedTimer, isParryingTimer, powerDashSpeed, dashBuffer, canAirDodgeTimer, airShieldTimer;
    protected float inputBuffer = .15f;
    [SerializeField] protected Transform leftHandTransform, rightHandTransform, GrabPosition, grabbedPositionTransform;
    [SerializeField] GameObject splatterPrefab;
    protected float punchRange = 3f;
    protected float punchRangeRight = 4f;
    protected float punchSpeed = 40f;
    protected float returnSpeed = 15f;
    int stocks = 4;
    PlayerController opponent;

    protected CameraShake cameraShake;

    protected SphereCollider leftHandCollider, rightHandCollider;
    [SerializeField] Animator animator;
    [SerializeField] protected Animator animatorUpdated;
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
        ParryStunned,
        AirDodging
    }


    protected virtual void Awake()
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
            case State.Dashing:
                HandleDash();
                HandleThrowingHands();
                HandleMovement();
                break;
            case State.Grabbed:
                HandleGrabbed();
                HandleThrowingHands();
                HandleShielding();
                break;
            case State.Grabbing:
                HandleGrabbing();
                break;
            case State.AirDodging:
                HandleAirDodge();
                HandleKnockback();
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
        if (animatorUpdated != null)
        {
            if (!shielding && state == State.Normal)
            {
                animatorUpdated.SetFloat("MoveSpeed", (movement.magnitude));
            }
            else
            {
                animatorUpdated.SetFloat("MoveSpeed", (0));
            }
        }
    }
    protected virtual void FixedHandleMovement()
    {
        Vector3 newVelocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, movement.z * moveSpeed);
        rb.velocity = newVelocity;
    }

    protected virtual void HandleThrowingHands()
    {
        if (animatorUpdated != null)
        {
            animatorUpdated.SetBool("punchingRight", (punchedRight));
            animatorUpdated.SetBool("punchingLeft", (punchedLeft));
            
        }
        if (punchedLeft && returningLeft == false)
        {
            
            punchedLeftTimer = 0;
            leftHandCollider.enabled = true;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector3(punchRange, -.4f, -.4f), punchSpeed * Time.deltaTime);
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
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(punchRangeRight, -.4f, .4f), punchSpeed * Time.deltaTime);
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

        if (state == State.Normal)
        {
            returnSpeed = 15f;
        }
        if (returningLeft &&  returningRight)
        {
            if (animator != null)
            {
                animator.SetBool("PunchRightAnimation", (false));
                animator.SetBool("RightHasReturned", (true));
            }
            returnSpeed = 6f;
        }
        if (state == State.Dashing)
        {
            if (animator != null)
            {
                animator.SetBool("PunchRightAnimation", (false));
                animator.SetBool("RightHasReturned", (true));
            }
            returnSpeed = 6f;
        }
    }

    public virtual void Knockback(float damage, Vector3 direction, PlayerController playerSent)
    {
        //this if is for if the opponent is grabbed
        if (opponent != null)
        {
            if (state == State.Grabbing)
            {
                opponent.Throw(transform.right);
            }
        }
        canAirDodgeTimer = 0f;

        currentPercentage += damage;
        brakeSpeed = 20f;
        // Debug.Log(damage + " damage");
        //Vector2 direction = new Vector2(rb.position.x - handLocation.x, rb.position.y - handLocation.y); //distance between explosion position and rigidbody(bluePlayer)
        //direction = direction.normalized;
        float knockbackValue = (14 * ((currentPercentage + damage) * (damage / 2)) / 150) + 14; //knockback that scales
        rb.velocity = new Vector3(direction.x * knockbackValue, 0, direction.z * knockbackValue);

        HitImpact(direction);
        state = State.Knockback;
    }
    void HandleKnockback()
    {
        EndPunchRight();
        EndPunchLeft();

        
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

        /*Vector3 lookTowards = new Vector3(oppositeForce.x, 0, oppositeForce.z);
        transform.right = lookTowards;*/
    }
    public virtual void EndPunchRight()
    {
        if (animator != null)
        {
            animator.SetBool("PunchRightAnimation", (false));
            animator.SetBool("RightHasReturned", (true));
        }
        punchedRight = false;
        returningRight = true;
        rightHandCollider.enabled = false;

    }
    void EndPunchLeft()
    {
        punchedLeft = false;
        returningLeft = true;
        leftHandCollider.enabled = false;
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
        if (!shielding && state != State.ParryState)
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
        shield.gameObject.SetActive(true);
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
        Respawn();
        stocks--;
        if (stocks >= 1)
        {
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
        
        StartCoroutine(cameraShake.Shake(.1f, .3f));
        StartCoroutine(FreezeFrames(.1f));
    }
    private IEnumerator FreezeFrames(float freezeTime)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(freezeTime);
        Time.timeScale = 1f;
    }
    void Dash(Vector3 dashDirection)
    {
        rightHandTransform.GetComponent<RightHand>().opponentTookDamage = false;
        isDashing = true;
        Debug.Log("Dash");
        shielding = false;
        punchedRight = true;
        returningRight = false;
        float dashDistance = 8f;
        transform.position += dashDirection * dashDistance;
        state = State.Dashing;
        GameObject splatter = Instantiate(splatterPrefab, rightHandTransform.position, Quaternion.identity);
        splatter.transform.right = transform.right;
    }
    void HandleDash()
    {
        if (rightHandTransform.localPosition.x > 1f)
        {
            rightHandCollider.enabled = true;
        }
        rb.velocity = Vector3.zero;
        if (returningRight && rightHandTransform.localPosition.x <= 1f)
        {
            rightHandCollider.enabled = false;
        }
        if (rightHandTransform.localPosition.x <= 0 && punchedRight == false)
        {
            isDashing = false;
            state = State.Normal;
        }
    }

    public void Grab(PlayerController opponent)
    {
        opponent.Grabbed(this);
        this.state = State.Grabbing;
        this.opponent = opponent;
    }

    void Grabbed(PlayerController playerGrabbing)
    {
        shielding = false;
        EndPunchLeft();
        EndPunchRight();
        grabbedPositionTransform = playerGrabbing.GrabPosition;
        this.transform.position = grabbedPositionTransform.position;
        this.state = State.Grabbed;
    }
    void HandleGrabbed()
    {
        shielding = false;
        rb.velocity = Vector3.zero;
        this.transform.position = grabbedPositionTransform.position;
    }
    void HandleGrabbing()
    {
        if (animator != null)
        {
            animator.SetBool("PunchRightAnimation", (false));
            animator.SetBool("RightHasReturned", (true));
        }
        returnSpeed = 15f;
        
        returningLeft = false;
        returningRight = false;
        rightHandCollider.enabled = false;
        leftHandCollider.enabled = false;
        rb.velocity = Vector3.zero;
        if (punchedLeft && !releasedLeft)
        {
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector3(punchRange, -.4f, -.4f), punchSpeed * Time.deltaTime);
        }
        if (punchedRight && !releasedRight)
        {
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(punchRange, -.4f, -.4f), punchSpeed * Time.deltaTime);
        }
        if (releasedLeft)
        {
            punchedLeft = false;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector3(0, 0, 0), returnSpeed * Time.deltaTime);
        }
        if (releasedRight)
        {
            punchedLeft = false;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(0, 0, 0), returnSpeed * Time.deltaTime);
        }
        if (releasedLeft && releasedRight)
        {
            returningLeft = true;
            returningRight = true;
            state = State.Normal;
            opponent.Throw(GrabPosition.right.normalized);
        }

    }
    public void Throw(Vector3 direction)
    {
        brakeSpeed = 20f;
        // Debug.Log(damage + " damage");
        //Vector2 direction = new Vector2(rb.position.x - handLocation.x, rb.position.y - handLocation.y); //distance between explosion position and rigidbody(bluePlayer)
        //direction = direction.normalized;
        float throwValue = (14 * ((120) * (3 / 2)) / 150) + 14; 
        rb.velocity = (direction * throwValue);
        HitImpact(direction);
        state = State.Knockback;
    }

    void AirDodge()
    {
        isParrying = true;
        shield.gameObject.SetActive(true);
        parryTimer = 0f;
        state = State.AirDodging;
    }
    void HandleAirDodge()
    {

        parryTimer += Time.deltaTime;
        if (parryTimer > parryTimerThreshold)
        {
            isParrying = false;
            shield.gameObject.SetActive(false);
        }
    }




    #region inputRegion
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
        airDodged = true;
        releasedAirDodged = false;
    }
    void OnReleaseShield()
    {
        releasedShield = true;
        pressedShield = false;
        airDodged = false;
        releasedAirDodged = true;
    }
    void OnDash()
    {
        pressedDash = true;
        releasedDash = false;
    }
    void OnReleaseDash()
    {
        pressedDash = false;
        releasedDash = true;
    }

    protected virtual void FaceLookDirection()
    {
        if (punchedLeft || punchedRight || leftHandTransform.localPosition.x > .1f && returningLeft || rightHandTransform.localPosition.x > .1f && returningRight) if(state != State.Grabbing)return;
       
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
        CheckForDash();
        CheckForAirDodge();
    }

    protected virtual void CheckForPunchLeft()
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
            if (splatterPrefab != null)
            {
                GameObject splatter = Instantiate(splatterPrefab, leftHandTransform.position, Quaternion.identity);
                splatter.transform.right = transform.right;
            }
            
            punchedLeft = true;
            punchedLeftTimer = 0;
        }
    }
    protected virtual void CheckForPunchRight()
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
        if (state == State.Dashing) return;

        if (punchedRightTimer > 0)
        {
            if (lookDirection.magnitude != 0)
            {
                Vector3 lookTowards = new Vector3(lookDirection.x, 0, lookDirection.y);
                transform.right = lookTowards;
            }
            if (splatterPrefab != null)
            {
                GameObject splatter = Instantiate(splatterPrefab, rightHandTransform.position, Quaternion.identity);
                splatter.transform.right = transform.right;
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
        if (punchedRight && punchedLeft)
        {
            shielding = false;
            return;
        }
        
        if (pressedShield)
        {
            if (canShieldAgainTimer > 0f) return;
            pressedShield = false;
            parryTimer = 0;
            shielding = true;
        }
    }
    void CheckForDash()
    {
        if (state != State.Dashing && isDashing)
        {
            isDashing = false;
        }
        //if you press dash set dash buffer = to input buffer
        if (pressedDash)
        {
            dashBuffer = inputBuffer;
            pressedDash = false;
        }

        //when dash button is released subtract time from dashbuffer so it only goes down when youre not pressing dash
        if (releasedDash)
        {
            dashBuffer -= Time.deltaTime;
        }

        //return area
        if (state != State.Normal) return;
        if (state == State.Dashing) return;
        //check if hasdashedtimer is good to go if not return

        //then if dash buffer is greater than 0 dash
        if (dashBuffer > 0)
        {
            dashBuffer = 0;
            Dash(transform.right.normalized);
        }
    }

    void CheckForAirDodge()
    {
        if (state == State.Knockback)
        {
            canAirDodgeTimer += Time.deltaTime;
            if (canAirDodgeTimer < .25f) return; //if the player has only been in knockback for x seconds return
            if (airDodged)
            {
                airDodged = false;
                AirDodge();
            }
        }
    }
    #endregion
}
