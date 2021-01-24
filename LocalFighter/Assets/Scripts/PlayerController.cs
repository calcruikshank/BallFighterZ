using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 mousePosition, movement, inputMovement, lastMoveDir, oppositeForce, dashPosition, joystickLook, powerDashTowards;


    public float moveSpeed, punchRange, punchSpeed, returnSpeed, currentPercentage, brakeSpeed, stunnedTimer, shieldSpeed, grabTimer, powerStunnedTimer, punchedRightTimer, punchedLeftTimer, inputBuffer, shieldLeftTimer, shieldRightTimer, powerDashSpeed, powerShieldTimer;

    public bool punchedRight, punchedLeft, returningRight, returningLeft, pummeledLeft, pummeledRight, isGrabbing, isGrabbed, shieldingRight, shieldingLeft, isBlockingRight, isBlockingLeft, readyToPummelRight, readyToPummelLeft, canDash, gameIsOver, isPowerShielding, startedPunchRight, startedPunchLeft, instantiatedArrow, respawned = false;

    public Transform rightHandTransform, leftHandTransform, grabPosition, grabbedPosition;
    public CircleCollider2D rightHandCollider, leftHandCollider;
    public PlayerController opponent;
    public int team, punchesToRelease;
    public GameManager gameManager;
    public TMP_Text percentageText;
    public int stocksLeft = 4;
    public TMP_Text stocksLeftText;
    public SpriteRenderer playerBody;
    public GameObject shield, arrowPointer, arrowPointerInstantiated;
    public float halfShieldRemaining, totalShieldRemaining, actualShield = 225f / 255f;
    public float dashedTimer, perfectShieldTimer, perfectShieldFrameData;
    public ScreenShake cameraShake;
    public GameObject teleportAnimation;
    public GameObject playerAnimatorBase;
    public AnimationTransformHandler animationTransformHandler;
    public Animator animator;
    public float canShieldAgainTimer, canShieldAgainTimerLeft = 0f;
    public float shieldAgainThreshold = .3f;

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
        FireGrabbed
    }

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        cameraShake = FindObjectOfType<ScreenShake>();
        rb = GetComponent<Rigidbody2D>();


        state = State.Normal;
    }

    public virtual void Start()
    {
        animationTransformHandler = Instantiate(playerAnimatorBase, transform.position, Quaternion.identity).GetComponent<AnimationTransformHandler>();
        animationTransformHandler.SetPlayer(this.gameObject);
        animator = animationTransformHandler.GetComponent<Animator>();
        totalShieldRemaining = 225f / 255f;
        if (gameManager != null)
        {
            gameManager.SetTeam(this);
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
        }

    }


    protected virtual void Update()
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
            case State.FireGrabbed:
                HandleGrabbed();
                HandleShielding();
                HandleThrowingHands();
                break;
            case State.Grabbing:
                HandleMovement();
                HandlePummel();
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


    protected virtual void FixedUpdate()
    {

        switch (state)
        {
            case State.Normal:
                FixedHandleMovement();
                break;
            case State.Grabbing:
                FixedHandleMovement();
                break;
            case State.PowerDashing:
                FixedHandlePowerDashing();
                break;
            case State.Dashing:
                FixedHandleDash();
                break;
        }
    }


    public virtual void HandleMovement()
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
        Time.timeScale = 1;

        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(movement.y));
            animator.SetFloat("SpeedX", Mathf.Abs(movement.x));
        }
        respawned = false;
    }

    public virtual void FixedHandleMovement()
    {
        rb.velocity = movement * moveSpeed;

        Time.timeScale = 1;
    }


    public virtual void Knockback(float damage, Vector2 direction)
    {
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
        float knockbackValue = (14 * ((currentPercentage + damage) * (damage / 2)) / 150) + 7; //knockback that scales
        rb.AddForce(direction * knockbackValue, ForceMode2D.Impulse);
        isGrabbed = false;
        //Debug.Log(currentPercentage + "current percentage");
        state = State.Knockback;
    }


    public void HandleKnockback()
    {
        if (opponent != null)
        {
            if (opponent.transform.position == grabPosition.position)
            {
                opponent.isGrabbed = false;
                opponent.state = State.Knockback;
            }
        }
        movement.x = inputMovement.x;
        movement.y = inputMovement.y;
        movement = movement;
        if (movement.x != 0 || movement.y != 0)
        {
            lastMoveDir = movement;
        }
        if (rb.velocity.magnitude <= 5f)
        {
            rb.velocity = new Vector2(0, 0);
            state = State.Normal;
        }
        if (rb.velocity.magnitude > 0)
        {
            oppositeForce = -rb.velocity;
            brakeSpeed = brakeSpeed + (100f * Time.deltaTime);
            rb.AddForce(oppositeForce * Time.deltaTime * brakeSpeed);
            rb.AddForce(lastMoveDir * Time.deltaTime * brakeSpeed * .5f); //DI
        }
    }

    public virtual void HandleThrowingHands()
    {
        punchedRightTimer -= Time.deltaTime;
        punchedLeftTimer -= Time.deltaTime;
        if (punchedLeftTimer > 0) punchedLeft = true;
        if (punchedRightTimer > 0) punchedRight = true;
        if (punchedRight && returningRight == false)
        {
            punchedRightTimer = 0;
            rightHandCollider.enabled = true;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(punchRange, .4f), punchSpeed * Time.deltaTime);
            if (rightHandTransform.localPosition.x >= punchRange)
            {
                returningRight = true;
            }
        }
        if (returningRight)
        {
            punchedRight = false;


            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);


            if (rightHandTransform.localPosition.x <= 1f)
            {
                rightHandCollider.enabled = false;
            }
            if (rightHandTransform.localPosition.x <= 0f)
            {
                returningRight = false;
            }
        }
        if (punchedLeft && returningLeft == false)
        {
            punchedLeftTimer = 0;
            leftHandCollider.enabled = true;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(punchRange, -.4f), punchSpeed * Time.deltaTime);
            if (leftHandTransform.localPosition.x >= punchRange)
            {
                returningLeft = true;
            }
        }
        if (returningLeft)
        {
            punchedLeft = false;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);

            if (leftHandTransform.localPosition.x <= 1f)
            {
                leftHandCollider.enabled = false;
            }
            if (leftHandTransform.localPosition.x <= 0f)
            {
                returningLeft = false;
            }
        }
        if (!returningLeft || !returningRight) returnSpeed = 4f;
        if (returningLeft && leftHandTransform.localPosition.x <= 1.25f && returningRight && rightHandTransform.localPosition.x <= 1.25f)
        {
            returnSpeed = 1f;
        }
    }

    public void HandlePummel()
    {
        if (opponent != null && opponent.respawned)
        {
            isGrabbing = false;
            opponent.transform.position = Vector3.zero;
            opponent.isGrabbed = false;
            state = State.Normal;
            opponent.rb.velocity = Vector3.zero;
            opponent.respawned = false;
        }
        returnSpeed = 25f;
        grabTimer += Time.deltaTime;
        if (grabTimer > (opponent.currentPercentage / 50f) + .2f && isGrabbing == true && opponent.isGrabbed)
        {
            returningLeft = true;
            returningRight = true;
            isGrabbing = false;
            opponent.rb.velocity = Vector3.zero;
            Debug.Log("grabtimer is greater");
            opponent.Throw(this.grabPosition.right);
            grabTimer = 0;
            state = State.Normal;
        }
        if (pummeledLeft)
        {
            leftHandCollider.enabled = true;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(punchRange, -.4f), punchSpeed * Time.deltaTime);
        }
        if (pummeledRight)
        {
            rightHandCollider.enabled = true;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(punchRange, .4f), punchSpeed * Time.deltaTime);
        }
        if (!pummeledRight)
        {
            rightHandCollider.enabled = false;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
            readyToPummelRight = true;
        }
        if (!pummeledLeft)
        {
            leftHandCollider.enabled = false;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
            readyToPummelLeft = true;
        }
        if (!pummeledLeft && !pummeledRight)
        {
            Debug.Log("didnt pummel left or right");
            returningLeft = true;
            returningRight = true;
            isGrabbing = false;
            opponent.rb.velocity = Vector3.zero;
            opponent.Throw(this.grabPosition.right);
            grabTimer = 0;
            state = State.Normal;
        }
        if (pummeledLeft && !pummeledRight)
        {
            leftHandCollider.enabled = false;
        }
        if (!pummeledLeft && pummeledRight)
        {
            rightHandCollider.enabled = false;
        }
        if (pummeledLeft && pummeledRight)
        {
            isGrabbing = true;
        }

    }
    public virtual void Grab(PlayerController opponentCheck)
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
        state = State.Grabbing;
    }
    public virtual void Grabbed(Transform player)
    {
        punchesToRelease = 0;
        grabbedPosition = player; //the transform that grabbed you is equal to the player that grabbed you grab position
        returningLeft = true;
        returningRight = true;
        state = State.Grabbed;
    }

    public void FireGrabbed(Transform player)
    {
        punchesToRelease = 4;
        grabbedPosition = player; //the transform that grabbed you is equal to the player that grabbed you grab position
        returningLeft = true;
        returningRight = true;
        state = State.FireGrabbed;
    }

    public void ShockGrabbed()
    {
        isGrabbed = true;
        isBlockingLeft = false;
        isBlockingRight = false;
        state = State.ShockGrabbed;
    }
    public void HandleGrabbed()
    {
        isGrabbed = true;
        isBlockingLeft = false;
        isBlockingRight = false;
        if (isGrabbed)
        {
            transform.position = grabbedPosition.position;
        }
    }
    public void Throw(Vector2 direction)
    {
        Debug.Log("Throw");
        isGrabbed = false;
        grabTimer = 0;
        moveSpeed = 12f;
        brakeSpeed = 20f;
        float knockbackValue = 20f;
        shieldRightTimer = 0;
        shieldLeftTimer = 0;
        shieldingLeft = false;
        shieldingRight = false;
        isBlockingLeft = false;
        isBlockingRight = false;
        rb.AddForce(direction * knockbackValue, ForceMode2D.Impulse);

        //Debug.Log(currentPercentage + "current percentage");
        state = State.Knockback;
    }


    public virtual void HandleShielding()
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
            moveSpeed = 12f;
        }


    }

    public virtual void EndPunchRight()
    {
        returningRight = true;
        punchedRight = false;
        punchedRightTimer = 0f;
        rightHandCollider.enabled = false;
        rightHandTransform.localScale = new Vector2(1, 1);
        pummeledLeft = false;
        //state = State.Normal;
    }

    public virtual void EndPunchLeft()
    {
        returningLeft = true;
        punchedLeft = false;
        punchedLeftTimer = 0f;
        leftHandCollider.enabled = false;
        leftHandTransform.localScale = new Vector2(1, 1);
        pummeledRight = false;
        //state = State.Normal;
    }
    public void Respawn()
    {
        grabTimer = 0;
        canDash = true;
        currentPercentage = 0;
        rb.velocity = Vector3.zero;
        transform.position = new Vector2(0, 0);
        percentageText.text = ((int)currentPercentage + "%");
        stocksLeftText.text = (stocksLeft.ToString());
        totalShieldRemaining = 225f / 255f;
        isGrabbed = false;
        isGrabbing = false;
        respawned = true;
        state = State.Normal;
    }

    public void HandleStunned()
    {

        //Debug.Log(stunnedTimer);

        shield.SetActive(false);
        leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
        rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
        leftHandTransform.localScale = new Vector2(1, 1);
        rightHandTransform.localScale = new Vector2(1, 1);
        rb.velocity = Vector3.zero;
        isBlockingLeft = false;
        isBlockingRight = false;
        totalShieldRemaining = 200f / 255f;
        actualShield = totalShieldRemaining;
        stunnedTimer += Time.deltaTime;
        if (stunnedTimer >= 3f)
        {
            state = State.Normal;
        }
    }

    public void TakePummelDamage()
    {
        currentPercentage++;
        percentageText.text = ((int)currentPercentage + "%");
    }

    public virtual void HandleDash()
    {
        returnSpeed = 2f;
        rb.velocity = Vector3.zero;
        shieldingRight = false;
        isBlockingRight = false;

        dashedTimer += Time.deltaTime;
        if (rightHandTransform.localPosition.x == 0 && punchedRight == false)
        {

            state = State.Normal;
            dashedTimer = 0f;
            canDash = true;

        }
        if (rightHandTransform.localPosition.x != 0 || punchedRight)
        {
            canDash = false;
        }
        //state = State.Normal;
    }

    public virtual void FixedHandleDash()
    {

    }
    public virtual void Dash(Vector3 direction)
    {
        if (canDash)
        {
            punchedRight = true;
            returningRight = false;
            //Debug.Log(direction);
            Instantiate(teleportAnimation, transform.position, Quaternion.identity);
            direction = direction.normalized;
            float dashDistance = 5f;
            transform.position += direction * dashDistance;
            state = State.Dashing;
            canDash = false;

        }

    }
    public void AddDamage(float damage)
    {
        currentPercentage += damage;
        percentageText.text = ((int)currentPercentage + "%");
    }
    public void EndGrab()
    {
        returningLeft = true;
        punchedLeft = false;
        leftHandCollider.enabled = false;
        leftHandTransform.localScale = new Vector2(1, 1);
        pummeledRight = false;
        returningRight = true;
        punchedRight = false;
        rightHandCollider.enabled = false;
        rightHandTransform.localScale = new Vector2(1, 1);
        pummeledLeft = false;
        state = State.Normal;
    }

    public void PowerShieldStun()
    {
        powerStunnedTimer = 0f;
        state = State.PowerShieldStunned;
    }

    public void HandlePowerShieldStunned()
    {
        rb.velocity = Vector3.zero;
        leftHandTransform.localScale = new Vector2(1f, 1f);
        rightHandTransform.localScale = new Vector2(1f, 1f);
        rightHandCollider.enabled = false;
        leftHandCollider.enabled = false;
        punchedLeft = false;
        punchedRight = false;
        powerStunnedTimer += Time.deltaTime;
        if (powerStunnedTimer >= .5f)
        {
            Debug.Log("change state to normal");
            state = State.Normal;
        }
    }
    public void PowerShield()
    {
        rb.velocity = Vector3.zero;
        instantiatedArrow = false;
        StartCoroutine(cameraShake.Shake(.03f, .3f));
        Instantiate(teleportAnimation, transform.position, Quaternion.identity);
        powerShieldTimer = 0;
        state = State.PowerShielding;
    }
    public void HandlePowerShielding()
    {
        Debug.Log("isPowerShielding");
        Time.timeScale = .2f;
        powerShieldTimer += Time.deltaTime;
        if (inputMovement.magnitude > .8f && instantiatedArrow == false)
        {
            arrowPointerInstantiated = Instantiate(arrowPointer, transform.position, transform.rotation);
            instantiatedArrow = true;
        }
        if (arrowPointerInstantiated != null)
        {
            if (inputMovement.magnitude <= .8f)
            {
                instantiatedArrow = false;
                Destroy(arrowPointerInstantiated);
            }
            arrowPointerInstantiated.transform.position = transform.position;
            arrowPointerInstantiated.transform.right = (inputMovement);
        }
        if (powerShieldTimer > .2f && inputMovement.magnitude > .8f)
        {
            shieldLeftTimer = 0;
            shieldRightTimer = 0;
            shieldingLeft = false;
            shieldingRight = false;
            isBlockingLeft = false;
            isBlockingRight = false;
            PowerDash(inputMovement);
            Destroy(arrowPointerInstantiated);
            return;
        }
        if (powerShieldTimer > .2f && inputMovement.magnitude <= .8f)
        {
            state = State.Normal;
        }
        if (inputMovement.magnitude > .8f && !shieldingLeft && !shieldingRight)
        {
            PowerDash(inputMovement);
            Destroy(arrowPointerInstantiated);
            return;
        }
        if (!shieldingLeft && !shieldingRight)
        {

            Destroy(arrowPointerInstantiated);
            state = State.Normal;
        }
        if (punchedRightTimer > 0 || punchedLeftTimer > 0)
        {

            Destroy(arrowPointerInstantiated);
            state = State.Normal;
        }


    }
    public void PowerDash(Vector2 powerDashDirection)
    {
        powerDashSpeed = 50f;
        powerDashTowards = powerDashDirection.normalized;
        state = State.PowerDashing;
    }

    public void HandlePowerDashing()
    {
        Time.timeScale = 1;
        float powerDashSpeedMulti = 5f;
        powerDashSpeed -= powerDashSpeed * powerDashSpeedMulti * Time.deltaTime;

        float powerDashMinSpeed = 10f;
        if (powerDashSpeed < powerDashMinSpeed)
        {
            state = State.Normal;
        }
    }

    public void FixedHandlePowerDashing()
    {
        rb.velocity = powerDashTowards * powerDashSpeed;
    }
    public void HandleUniversal()
    {
        if (arrowPointerInstantiated != null)
        {
            Destroy(arrowPointerInstantiated);
        }
    }
    public void HandleShockGrabbed()
    {
        if (isGrabbed)
        {
            rb.velocity = Vector3.zero;
        }
    }




    #region InputRegion
    void OnKeyboardMove(InputValue value)
    {
        inputMovement = value.Get<Vector2>().normalized;
    }
    public virtual void OnRightStickDash(InputValue value) //this actually dashes based on right stick input
    {
        if (state != State.Normal) return;
        if (value != null)
        {
            dashPosition = value.Get<Vector2>();
        }
        if (dashPosition.magnitude >= .9f)
        {

            Dash(dashPosition.normalized);


        }
        //if(usingMouse) FaceMouse();

    }

    void OnMouseMove(InputValue value)
    {

        mousePosition = value.Get<Vector2>();
        FaceMouse();
    }
    void OnKeyboardMove1(InputValue value)
    {
        //Debug.Log(joystickLook);
        joystickLook = value.Get<Vector2>();
        FaceJoystick();
    }
    public virtual void FaceMouse()
    {
        if (state == State.Dashing) return;
        if (state == State.PowerShieldStunned) return;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.right = direction;
    }

    public virtual void FaceJoystick()
    {
        if (punchedLeft || punchedRight || leftHandTransform.localPosition.x > .1f && returningLeft || rightHandTransform.localPosition.x > .1f && returningRight) return;
        if (state == State.Dashing) return;
        if (state == State.PowerShieldStunned) return;
        Vector2 joystickPosition = joystickLook.normalized;
        if (joystickPosition.x != 0 || joystickPosition.y != 0)
        {
            Vector2 lastLookedPosition = joystickPosition;
            //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            transform.right = lastLookedPosition;
        }


    }
    public virtual void OnPunchRight()
    {
        pummeledRight = true;
        if (state == State.Grabbing) return;
        if (state == State.FireGrabbed) return;
        if (state == State.Stunned) return;
        if (state == State.Dashing) return;
        if (shieldingLeft || shieldingRight) return;
        //if (state == State.Knockback) return;
        Vector2 joystickPosition = joystickLook.normalized;
        if (joystickPosition.x != 0 || joystickPosition.y != 0)
        {
            Vector2 lastLookedPosition = joystickPosition;
            //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            transform.right = lastLookedPosition;
        }
        punchedRightTimer = inputBuffer;
        if (returningRight) return;
        //punchedRight = true;
        shieldingRight = false;
    }
    public virtual void OnPunchLeft()
    {
        pummeledLeft = true;
        if (state == State.Grabbing) return;
        if (state == State.FireGrabbed) return;
        if (state == State.Stunned) return;
        if (state == State.Dashing) return;
        if (shieldingLeft || shieldingRight) return;

        //if (state == State.Knockback) return;
        Vector2 joystickPosition = joystickLook.normalized;
        if (joystickPosition.x != 0 || joystickPosition.y != 0)
        {
            Vector2 lastLookedPosition = joystickPosition;
            //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            transform.right = lastLookedPosition;
        }
        punchedLeftTimer = inputBuffer;
        if (returningLeft) return;
        //punchedLeft = true;
        shieldingLeft = false;
    }
    public virtual void OnReleasePunchRight()
    {
        pummeledRight = false;
    }
    public virtual void OnReleasePunchLeft()
    {
        pummeledLeft = false;
    }

    void OnShieldRight()
    {
        if (state == State.FireGrabbed) return;
        if (state == State.Grabbed) return;
        if (state == State.Dashing) return;
        if (state == State.Knockback) return;
        shieldRightTimer = inputBuffer;
        if (punchedRight || returningRight)
        {
            return;
        }
        //shieldingRight = true;
    }

    void OnShieldLeft()
    {
        if (state == State.FireGrabbed) return;
        if (state == State.Grabbed) return;
        if (state == State.Dashing) return;
        if (state == State.Knockback) return;

        shieldLeftTimer = inputBuffer;

        //shieldingLeft = true;
    }

    void OnReleaseShieldRight()
    {
        shieldRightTimer = 0;
        shieldingRight = false;
    }
    void OnReleaseShieldLeft()
    {

        shieldLeftTimer = 0;
        shieldingLeft = false;
    }

    void OnRestart()
    {
        if (gameManager.gameIsOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    protected virtual void OnMouseDash()
    {
        if (state != State.Normal) return;
        
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        //Vector2 direction = inputMovement.normalized;
        Dash(direction.normalized);


    }

    void OnShieldBoth()
    {
        if (state == State.FireGrabbed) return;
        if (state == State.ShockGrabbed) return;
        if (state == State.Grabbed) return;
        if (state == State.Dashing) return;
        if (state == State.Knockback) return;
        if (punchedRight || returningRight)
        {
            return;
        }

        shieldingRight = true;
        shieldingLeft = true;
    }

    void OnReleaseShieldBoth()
    {
        shieldLeftTimer = 0;
        shieldRightTimer = 0;
        shieldingLeft = false;
        shieldingRight = false;
    }
    #endregion

}
