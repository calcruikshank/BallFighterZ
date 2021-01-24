using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
public class FirePlayer : PlayerController
{

    public Vector2 rightStickLook;
    public Rigidbody2D thrownRightFireballRB, thrownLeftFireballRB;
    public GameObject thrownRightFireball;
    public GameObject thrownLeftFireball;
    public GameObject thrownFireballPrefab, explosionFireballPrefab;
    public bool threwRight, threwLeft = false;
    public float fireBallSpeed = 30f;
    public Vector2 oppositeRightHammerForce, oppositeLeftHammerForce;
    public float dashTimer;
    private float dashSpeed = 30f;
    private Vector2 lastLookedPosition, dashLookPosition;
    bool dashingIdle, throwPlayer = false;
    public bool firePlayerGrabbing;
    float dashingTimer;

    public override void Start()
    {
        if (playerAnimatorBase != null)
        {
            animationTransformHandler = Instantiate(playerAnimatorBase, transform.position, Quaternion.identity).GetComponent<AnimationTransformHandler>();
            animationTransformHandler.SetPlayer(this.gameObject);
            animator = animationTransformHandler.GetComponent<Animator>();
        }

        totalShieldRemaining = 225f / 255f;
        gameManager.SetTeam((PlayerController)this);
        if (team == 0)
        {
            Color redColor = new Color(255f / 255f, 97f / 255f, 96f / 255f);
            playerBody.material.SetColor("_Color", redColor);
            shield.GetComponent<SpriteRenderer>().material.SetColor("_Color", redColor);
        }
        if (team == 1)
        {
            Color blueColor = new Color(124f / 255f, 224f / 255f, 224f / 255f);
            playerBody.material.SetColor("_Color", blueColor);
            shield.GetComponent<SpriteRenderer>().material.SetColor("_Color", blueColor);
        }
        rightHandCollider = rightHandTransform.GetComponent<CircleCollider2D>();
        leftHandCollider = leftHandTransform.GetComponent<CircleCollider2D>();
        canDash = true;
        stocksLeft = 4;
        stocksLeftText.text = (stocksLeft.ToString());
        dashTimer = 3;

    }

    /*protected override void Update()
    {
        switch (state)
        {
            case State.Normal:
                HandleMovement();
                HandleThrowingHands();
                HandleShielding();
                HandleUniversal();
                break;
            case State.Knockback:
                HandleKnockback();
                HandleThrowingHands();
                HandleShielding();
                break;
            case State.Grabbed:
                HandleGrabbed();
                HandleShielding();
                HandleThrowingHands();
                break;
            case State.Grabbing:
                HandleDash();
                HandleThrowingHands();
                break;
            case State.Stunned:
                HandleStunned();
                break;
            case State.Dashing:
                HandleDash();
                HandleThrowingHands();
                HandleShielding();
                break;
            case State.PowerShieldStunned:
                HandlePowerShieldStunned();
                break;
            case State.PowerShielding:
                HandlePowerShielding();
                break;
            case State.PowerDashing:
                HandlePowerDashing();
                HandleThrowingHands();
                HandleShielding();
                break;
            case State.ShockGrabbed:
                HandleShockGrabbed();
                HandleShielding();
                HandleThrowingHands();
                break;
        }
    }


    protected override void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                FixedHandleMovement();
                break;
            case State.Grabbing:
                FixedHandleDash();
                break;
            case State.PowerDashing:
                FixedHandlePowerDashing();
                break;
            case State.Dashing:
                FixedHandleDash();
                break;
        }
    }*/
    public override void HandleShielding()
    {
        canShieldAgainTimer += Time.deltaTime;
        canShieldAgainTimerLeft += Time.deltaTime;
        shieldLeftTimer -= Time.deltaTime;
        if (shieldLeftTimer > 0 && returningLeft == false && punchedLeft == false && canShieldAgainTimerLeft > shieldAgainThreshold)
        {
            shieldingLeft = true;
            canShieldAgainTimerLeft = 0f;
        }
        shieldRightTimer -= Time.deltaTime;
        if (shieldRightTimer > 0 && returningRight == false && punchedRight == false && canShieldAgainTimer > shieldAgainThreshold)
        {
            shieldingRight = true;
            canShieldAgainTimer = 0f;
        }
        stunnedTimer = 0;
        if (actualShield <= 25f / 255f)
        {
            state = State.Stunned;
        }
        //Debug.Log(isBlockingLeft + " left" + isBlockingRight + " right");
        if (shieldingRight && !shieldingLeft)
        {



            //rightHandTransform.localScale = new Vector2(.75f, 1.25f);
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(0, .4f), punchSpeed * Time.deltaTime);

        }

        if (!shieldingRight)
        {
            rightHandCollider.radius = .5f;
            rightHandCollider.isTrigger = true;
            //rightHandTransform.localScale = new Vector2(1, 1);
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
        }

        if (shieldingLeft && shieldingRight)
        {
            //leftHandTransform.localScale = new Vector2(.75f, 1.25f);
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(0, -.4f), punchSpeed * Time.deltaTime);
            //rightHandTransform.localScale = new Vector2(.75f, 1.25f);
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(0, .4f), punchSpeed * Time.deltaTime);
        }

        if (shieldingLeft && !shieldingRight)
        {

            //leftHandTransform.localScale = new Vector2(.75f, 1.25f);
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(0, -.4f), punchSpeed * Time.deltaTime);
        }

        if (!shieldingLeft)
        {
            leftHandCollider.radius = .5f;
            leftHandCollider.isTrigger = true;
            //leftHandTransform.localScale = new Vector2(1, 1);
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
        }
        if (rightHandTransform.localPosition.x == 0 && rightHandTransform.localPosition.y == .4f)
        {
            isBlockingRight = true;

        }
        if (rightHandTransform.localPosition.y <= .2f)
        {
            isBlockingRight = false;
        }
        if (leftHandTransform.localPosition.y <= -.2f)
        {
            isBlockingLeft = false;
        }
        if (leftHandTransform.localPosition.x == 0 && leftHandTransform.localPosition.y == -.4f)
        {
            isBlockingLeft = true;

        }
        if (isBlockingRight && !isBlockingLeft)
        {
            perfectShieldTimer += Time.deltaTime;
            if (perfectShieldTimer < perfectShieldFrameData)
            {
                isPowerShielding = true;
            }
            else
            {
                isPowerShielding = false;
            }
            totalShieldRemaining -= (50f / 255f) * Time.deltaTime;
            //Debug.Log(halfShieldRemaining);
            //Debug.Log(totalShieldRemaining);
            shield.SetActive(true);
            halfShieldRemaining = totalShieldRemaining / 2;
            actualShield = halfShieldRemaining;
            Color tmp = shield.GetComponent<SpriteRenderer>().color;
            tmp.a = actualShield;
            shield.GetComponent<SpriteRenderer>().color = tmp;
            /*rightHandCollider.enabled = true;
            rightHandCollider.radius = .75f;
            rightHandCollider.isTrigger = false;*/
            moveSpeed = 0f;

        }
        if (isBlockingLeft && !isBlockingRight)
        {
            perfectShieldTimer += Time.deltaTime;
            if (perfectShieldTimer < perfectShieldFrameData)
            {
                isPowerShielding = true;
            }
            else
            {
                isPowerShielding = false;
            }
            totalShieldRemaining -= (20f / 255f) * Time.deltaTime;
            //Debug.Log(halfShieldRemaining);
            //Debug.Log(totalShieldRemaining);
            shield.SetActive(true);
            halfShieldRemaining = totalShieldRemaining / 2;
            actualShield = halfShieldRemaining;
            Color tmp = shield.GetComponent<SpriteRenderer>().color;
            tmp.a = actualShield;
            shield.GetComponent<SpriteRenderer>().color = tmp;
            /*leftHandCollider.enabled = true;
            leftHandCollider.radius = .75f;
            leftHandCollider.isTrigger = false;*/
            moveSpeed = 0f;

        }
        if (isBlockingLeft && isBlockingRight)
        {

            actualShield = totalShieldRemaining;
            totalShieldRemaining -= (20f / 255f) * Time.deltaTime;

            shield.SetActive(true);
            Color tmp = shield.GetComponent<SpriteRenderer>().color;
            tmp.a = actualShield;
            shield.GetComponent<SpriteRenderer>().color = tmp;
            moveSpeed = 0f;
            perfectShieldTimer += Time.deltaTime;
            if (perfectShieldTimer < perfectShieldFrameData)
            {
                isPowerShielding = true;
            }
            else
            {
                isPowerShielding = false;
            }

        }
        if (!isBlockingRight && !isBlockingLeft)
        {
            perfectShieldTimer = 0f;
            isPowerShielding = false;
            shield.SetActive(false);
            if (totalShieldRemaining < 225f / 255f)
            {
                totalShieldRemaining += 5f / 255f * Time.deltaTime;
            }
            moveSpeed = 10f;
        }


    }


    public override void HandleMovement()
    {
        movement.x = inputMovement.x;
        movement.y = inputMovement.y;
        movement = movement;
        if (movement.x != 0 || movement.y != 0)
        {
            lastMoveDir = movement;
        }
        stunnedTimer = 0;
        isGrabbed = false;
        dashedTimer = 0f;
        canDash = true;

        dashingIdle = false;
        returnSpeed = 3f;
    }
    public override void HandleThrowingHands()
    {
        if (state != State.Dashing)
        {
            dashingIdle = false;
            rightStickLook = Vector3.zero;

            rightHandCollider.enabled = false;
            leftHandCollider.enabled = false;
            isGrabbing = false;
            if (opponent != null)
            {
                opponent.isGrabbed = false;
            }
        }
        punchedRightTimer -= Time.deltaTime;
        punchedLeftTimer -= Time.deltaTime;
        if (punchedLeftTimer > 0 && leftHandTransform.localPosition.x <= 0) punchedLeft = true;
        if (punchedRightTimer > 0 && rightHandTransform.localPosition.x <= 0) punchedRight = true;

        if (punchedRight && returningRight == false)
        {

            punchedRightTimer = 0;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(punchRange, .7f), punchSpeed * Time.deltaTime);
            if (rightHandTransform.localPosition.x == punchRange && threwRight == false)
            {
                thrownRightFireball = Instantiate(thrownFireballPrefab, rightHandTransform.position, transform.rotation);
                thrownRightFireball.GetComponent<Fireball>().SetPlayer(this, 0);
                thrownRightFireballRB = thrownRightFireball.GetComponent<Rigidbody2D>();
                thrownRightFireballRB.AddForce(transform.right * fireBallSpeed, ForceMode2D.Impulse);
                threwRight = true;

            }
        }


        if (returningRight)
        {

            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
            punchedRight = false;
            if (rightHandTransform.localPosition.x <= 0f)
            {
                returningRight = false;
                threwRight = false;
            }
        }





        if (punchedLeft && returningLeft == false)
        {

            punchedLeftTimer = 0;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(punchRange, -.7f), punchSpeed * Time.deltaTime);
            if (leftHandTransform.localPosition.x == punchRange && threwLeft == false)
            {
                thrownLeftFireball = Instantiate(thrownFireballPrefab, leftHandTransform.position, transform.rotation);
                thrownLeftFireball.GetComponent<Fireball>().SetPlayer(this, 1);
                thrownLeftFireballRB = thrownLeftFireball.GetComponent<Rigidbody2D>();
                thrownLeftFireballRB.AddForce(transform.right * fireBallSpeed, ForceMode2D.Impulse);
                threwLeft = true;

            }
        }

        if (returningLeft)
        {

            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
            punchedLeft = false;
            if (leftHandTransform.localPosition.x <= 0f)
            {
                returningLeft = false;
                threwLeft = false;
            }
        }


    }

    


    public override void OnReleasePunchRight()
    {
        if (thrownRightFireball == null && punchedRight == true)
        {
            GameObject instantiatedExplosion = Instantiate(explosionFireballPrefab, rightHandTransform.position, this.transform.rotation);
            instantiatedExplosion.GetComponent<ExplosionScript>().SetPlayer(this);
        }
        if (thrownRightFireball != null)
        {
            thrownRightFireball.GetComponent<Fireball>().DestroyRightFireball();
        }
        returningRight = true;

    }
    public override void OnReleasePunchLeft()
    {
        if (thrownLeftFireball == null && punchedLeft == true)
        {
            GameObject instantiatedExplosion = Instantiate(explosionFireballPrefab, leftHandTransform.position, this.transform.rotation);
            instantiatedExplosion.GetComponent<ExplosionScript>().SetPlayer(this);
        }
        if (thrownLeftFireball != null)
        {
            thrownLeftFireball.GetComponent<Fireball>().DestroyLeftFireball();
        }
        returningLeft = true;
    }

    public override void OnPunchRight()
    {
        if (shieldingLeft || shieldingRight) return;

        if (state == State.FireGrabbed) return;
        if (state == State.Grabbing) return;
        if (state == State.Stunned) return;
        if (state == State.Dashing) return;
        //if (state == State.Knockback) return;
        Vector2 joystickPosition = joystickLook.normalized;
        if (joystickPosition.x != 0 || joystickPosition.y != 0 && rightStickLook.magnitude == 0)
        {
            Vector2 lastLookedPosition = joystickPosition;
            //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            transform.right = lastLookedPosition;
        }
        if (rightStickLook.magnitude != 0)
        {
            Vector2 lastLookedPosition = rightStickLook;
            //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            transform.right = lastLookedPosition;
        }
        punchedRightTimer = inputBuffer;
        //punchedRight = true;
        shieldingRight = false;
    }
    public override void OnPunchLeft()
    {
        if (shieldingLeft || shieldingRight) return;

        if (state == State.FireGrabbed) return;
        if (state == State.Grabbing) return;
        if (state == State.Stunned) return;
        if (state == State.Dashing) return;
        //if (state == State.Knockback) return;
        Vector2 joystickPosition = joystickLook.normalized;
        if (joystickPosition.x != 0 || joystickPosition.y != 0 && rightStickLook.magnitude == 0)
        {
            Vector2 lastLookedPosition = joystickPosition;
            //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            transform.right = lastLookedPosition;
        }
        if (rightStickLook.magnitude != 0)
        {
            Vector2 lastLookedPosition = rightStickLook;
            //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            transform.right = lastLookedPosition;
        }
        punchedLeftTimer = inputBuffer;
        //punchedRight = true;
        shieldingLeft = false;
    }

    public override void HandleDash()
    {
        dashLookPosition = Vector3.RotateTowards(transform.right, rightStickLook.normalized, 8 * Time.deltaTime, 0f);

        //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.right = dashLookPosition;
        if (!returningLeft)
        {
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(punchRange, -.4f), punchSpeed * Time.deltaTime);
            leftHandCollider.enabled = true;
        }
        if (!returningRight)
        {
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(punchRange, .4f), punchSpeed * Time.deltaTime);
            rightHandCollider.enabled = true;
        }
        float powerDashSpeedMulti = 1.5f;
        movement.x = dashLookPosition.x;
        movement.y = dashLookPosition.y;
        if (movement.x != 0 || movement.y != 0)
        {
            lastMoveDir = movement;
        }
        stunnedTimer = 0;
        returnSpeed = 8f;
        isGrabbed = false;
        dashedTimer = 0f;

        float powerDashMinSpeed = 20f;
        if (dashSpeed > powerDashMinSpeed)
        {
            dashSpeed -= dashSpeed * powerDashSpeedMulti * Time.deltaTime;
            dashingIdle = false;
        }
        if (dashSpeed <= powerDashMinSpeed)
        {
            dashSpeed -= dashSpeed * powerDashSpeedMulti * Time.deltaTime;
            dashingIdle = true;
            if (dashSpeed <= 15f)
            {
                if (isGrabbing && opponent != null && !throwPlayer && opponent.isGrabbed && isGrabbing)
                {
                    opponent.rb.velocity = Vector3.zero;
                    opponent.Throw(this.grabPosition.right);
                    Debug.Log("Threw Player");
                    isGrabbing = false;
                }
                rightHandCollider.enabled = false;
                leftHandCollider.enabled = false;

                returningRight = true;
                returningLeft = true;
            }
            if (dashSpeed < 10)
            {
                rightStickLook = Vector3.zero;
                dashingIdle = false;
                state = State.Normal;
            }
        }
    }

    public override void FixedHandleDash()
    {
        rb.velocity = movement * dashSpeed;

        Time.timeScale = 1;
    }

    public override void Knockback(float damage, Vector2 direction)
    {
        
        dashingIdle = false;
        rightStickLook = Vector3.zero;

        rightHandCollider.enabled = false;
        leftHandCollider.enabled = false;
        isGrabbing = false;
        if (opponent != null && opponent.isGrabbed)
        {
            opponent.isGrabbed = false;
            opponent.Throw(this.grabPosition.right);
        }
        StartCoroutine(cameraShake.Shake(.04f, .4f));
        EndPunchLeft();
        EndPunchRight();
        shieldingLeft = false;
        shieldingRight = false;
        isBlockingLeft = false;
        isBlockingRight = false;
        shieldRightTimer = 0;
        shieldLeftTimer = 0;
        currentPercentage += damage;
        percentageText.text = ((int)currentPercentage + "%");
        brakeSpeed = 20f;
        // Debug.Log(damage + " damage");
        //Vector2 direction = new Vector2(rb.position.x - handLocation.x, rb.position.y - handLocation.y); //distance between explosion position and rigidbody(bluePlayer)
        //direction = direction.normalized;
        rb.velocity = Vector3.zero;
        float knockbackValue = (14 * ((currentPercentage + damage) * (damage / 2)) / 150) + 7; //knockback that scales
        rb.AddForce(direction * knockbackValue, ForceMode2D.Impulse);
        isGrabbed = false;
        //Debug.Log(currentPercentage + "current percentage");
        state = State.Knockback;
    }
    void OnReleaseDash()
    {

    }
    void OnReleaseDashController()
    {

    }

    public override void OnRightStickDash(InputValue value)
    {
        if (punchedLeft || punchedRight) return;
        if (state != State.Normal && state != State.Dashing) return;
        if (dashingIdle)
        {
            return;
        }
        rightStickLook = value.Get<Vector2>();
        if (rightStickLook.magnitude >= .8f)
        {

            if (state == State.Dashing) return;
            transform.right = rightStickLook.normalized;
            returningRight = false;
            returningLeft = false;
            dashSpeed = 30f;
            dashingTimer = 0f;
            state = State.Dashing;
        }

    }
    public override void Grab(PlayerController opponentCheck)
    {
        moveSpeed = 5f;
        returningLeft = false;
        returningRight = false;
        punchedLeft = false;
        punchedRight = false;
        isGrabbing = true;
        opponent = opponentCheck;
        leftHandTransform.localPosition = new Vector2(punchRange, -.4f);
        rightHandTransform.localPosition = new Vector2(punchRange, .4f);
    }

    public override void FaceJoystick()
    {
        if (threwLeft || threwRight) return;
        if (state == State.PowerShieldStunned) return;
        if (rightStickLook.magnitude > .8f && state == State.Dashing) return;
        if (state == State.Dashing) return;
        Vector2 joystickPosition = joystickLook.normalized;
        if (joystickPosition.x != 0 || joystickPosition.y != 0)
        {
            lastLookedPosition = joystickPosition;
            //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            transform.right = lastLookedPosition;
        }
    }
    public override void FaceMouse()
    {

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        if (dashingIdle)
        {
            return;
        }
        rightStickLook = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        if (state == State.Dashing) return;
        if (state == State.PowerShieldStunned) return;
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.right = direction;
    }
    protected override void OnMouseDash()
    {
        if (punchedLeft || punchedRight) return;
        if (state != State.Normal) return;
        if (dashingIdle)
        {
            return;
        }

        if (state == State.Dashing) return;
        returningRight = false;
        returningLeft = false;
        dashSpeed = 30f;
        dashingTimer = 0f;
        state = State.Dashing;

    }
}
