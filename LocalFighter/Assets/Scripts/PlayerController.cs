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

    public bool punchedRight, punchedLeft, returningRight, returningLeft, pummeledLeft, pummeledRight, isGrabbing, isGrabbed, shieldingRight, shieldingLeft, isBlockingRight, isBlockingLeft, readyToPummelRight, readyToPummelLeft, canDash, isPowerShielding, startedPunchRight, startedPunchLeft, instantiatedArrow, respawned = false;

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
    public float shieldAgainThreshold = .25f;
    [SerializeField] float moveSpeedSetter = 12f;
    protected Vector2 lookPositionRightStick;
    [SerializeField] protected GameObject redHand, blueHand;
    [SerializeField] protected GameObject redShield, blueShield;
    protected bool airShielding, canAirShield, pressedAirShield, pressedAirShieldWhileInKnockback;
    protected float airShieldTimer, canAirShieldTimer, canAirShieldThreshold, airPowerShieldTimer;
    [SerializeField] protected GameObject airShieldAnimation, airShieldInstantiated, controlsMenu, controlsMenuInstantiated;
    protected bool releaseShieldBuffer, pressedRight, pressedLeft, pressedShieldBoth, releasedShieldBoth, releasedRight, releasedLeft = false;



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
        canAirShieldThreshold = .1f;
        animationTransformHandler = Instantiate(playerAnimatorBase, transform.position, Quaternion.identity).GetComponent<AnimationTransformHandler>();
        animationTransformHandler.SetPlayer(this.gameObject);
        animator = animationTransformHandler.GetComponent<Animator>();
        totalShieldRemaining = 225f / 255f;
        if (gameManager != null)
        {
            gameManager.SetTeam(this);
            if (team % 2 == 0)
            {
                GameObject redHandObject = Instantiate(redHand, Vector3.zero, Quaternion.identity);
                redHandObject.transform.SetParent(rightHandTransform, false);
                GameObject redHandObject1 = Instantiate(redHand, Vector3.zero, Quaternion.identity);
                redHandObject1.transform.SetParent(leftHandTransform, false);

                shield = Instantiate(redShield, Vector3.zero, Quaternion.identity);
                shield.transform.SetParent(this.transform, false);

                Color redColor = new Color(255f / 255f, 97f / 255f, 96f / 255f);
                playerBody.material.SetColor("_Color", redColor);

                Debug.Log(team + "make a red shield");
                //shield.GetComponent<SpriteRenderer>().material.SetColor("_Color", redColor);
            }
            if (team % 2 == 1)
            {
                GameObject blueHandObject = Instantiate(blueHand, Vector3.zero, Quaternion.identity);
                blueHandObject.transform.SetParent(rightHandTransform, false);
                GameObject blueHandObject1 = Instantiate(blueHand, Vector3.zero, Quaternion.identity);
                blueHandObject1.transform.SetParent(leftHandTransform, false);

                shield = Instantiate(blueShield, Vector3.zero, Quaternion.identity);
                shield.transform.SetParent(this.transform, false);

                Color blueColor = new Color(124f / 255f, 224f / 255f, 224f / 255f);
                playerBody.material.SetColor("_Color", blueColor);
                //shield.GetComponent<SpriteRenderer>().material.SetColor("_Color", blueColor);
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
                HandleAirShielding();
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
        CheckForInputs();
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

        animationTransformHandler.SetEmittingToFalse();
    }

    public virtual void FixedHandleMovement()
    {
        rb.velocity = movement * moveSpeed;

        Time.timeScale = 1;
    }


    public virtual void Knockback(float damage, Vector2 direction)
    {
        if (respawned == false)
        {
            animationTransformHandler.EnableEmitter();
        }
        canAirShield = true;
        pressedAirShieldWhileInKnockback = false;
        canAirShieldTimer = 0f;
        //canAirShieldThreshold = .5f;
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
        //canAirShieldThreshold = knockbackValue * .01f;
        rb.AddForce(direction * knockbackValue, ForceMode2D.Impulse);
        isGrabbed = false;
        //Debug.Log(currentPercentage + "current percentage");
        state = State.Knockback;
    }


    public virtual void HandleKnockback()
    {
        animationTransformHandler.SetEmittingToTrue();
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
        if (rb.velocity.magnitude <= 5)
        {
            
            rb.velocity = new Vector2(0, 0);
            state = State.Normal;
            if (releaseShieldBuffer)
            {
                releaseShieldBuffer = false;
                shieldingLeft = false;
                shieldingRight = false;
            }
        }
        if (rb.velocity.magnitude > 0)
        {
            
            oppositeForce = -rb.velocity;
            brakeSpeed = brakeSpeed + (100f * Time.deltaTime);
            rb.AddForce(oppositeForce * Time.deltaTime * brakeSpeed);
            rb.AddForce(movement * .05f); //DI
        }
    }

    public virtual void HandleThrowingHands()
    {
        
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
        if (!returningLeft || !returningRight && state != State.Dashing) returnSpeed = 4f;
        if (returningLeft && leftHandTransform.localPosition.x <= 1.25f && returningRight && rightHandTransform.localPosition.x <= 1.25f || state == State.Dashing)
        {
            returnSpeed = 1.5f;
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
        if (isGrabbed && opponent != null)
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
        moveSpeed = 0f;
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
        if (isGrabbing && opponent != null)
        {
            returningLeft = true;
            returningRight = true;
            isGrabbing = false;
            opponent.rb.velocity = Vector3.zero;
            Debug.Log("grabtimer is greater");
            grabTimer = 0;
            state = State.Normal;
        }
        isGrabbed = true;
        isBlockingLeft = false;
        isBlockingRight = false;
        if (isGrabbed)
        {
            transform.position = grabbedPosition.position;
        }
    }
    public virtual void Throw(Vector2 direction)
    {
        canAirShield = true;
        pressedAirShieldWhileInKnockback = false;
        canAirShieldTimer = 0f;
        //canAirShieldThreshold = .5f;
        Debug.Log("Throw");
        StartCoroutine(cameraShake.Shake(.04f, .4f));
        isGrabbed = false;
        EndPunchLeft();
        EndPunchRight();
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
        //canAirShieldThreshold = knockbackValue * .01f;
        //canAirShieldThreshold = knockbackValue * .01f;
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
            if (punchedLeft || punchedRight || returningRight || returningLeft)
            {
                moveSpeed = moveSpeedSetter - 4f;
                return;
            }

            moveSpeed = moveSpeedSetter;
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
    public virtual void Respawn()
    {
        if (stocksLeft <= 0 && this.gameObject != null)
        {
            Destroy(animationTransformHandler.gameObject);
            Destroy(this.gameObject);
        }
        shieldingLeft = false;
        isBlockingLeft = false;
        shieldingRight = false;
        isBlockingRight = false;
        animationTransformHandler.SetEmittingToFalse();
        animationTransformHandler.DisableEmitter();
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
        returnSpeed = 1f;
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
            shieldingLeft = false;
            shieldingRight = false;
            isBlockingLeft = false;
            isBlockingRight = false;
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
        if (releaseShieldBuffer)
        {
            releaseShieldBuffer = false;
            shieldLeftTimer = 0;
            shieldRightTimer = 0;
            shieldingLeft = false;
            shieldingRight = false;
        }
        rb.velocity = Vector3.zero;
        instantiatedArrow = false;
        StartCoroutine(cameraShake.Shake(.03f, .3f));
        Instantiate(teleportAnimation, transform.position, Quaternion.identity);
        powerShieldTimer = 0;
        state = State.PowerShielding;
    }
    public void HandlePowerShielding()
    {
        if (releaseShieldBuffer)
        {
            releaseShieldBuffer = false;
            shieldLeftTimer = 0;
            shieldRightTimer = 0;
            shieldingLeft = false;
            shieldingRight = false;
        }
        Debug.Log("isPowerShielding");
        Time.timeScale = .2f;
        powerShieldTimer += Time.deltaTime;
        if (inputMovement.magnitude > .8f && instantiatedArrow == false)
        {
            if (arrowPointerInstantiated != null)
            {
                Destroy(arrowPointerInstantiated);
            }
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
            shieldLeftTimer = 0;
            shieldRightTimer = 0;
            shieldingLeft = false;
            shieldingRight = false;
            isBlockingLeft = false;
            isBlockingRight = false;
            PowerDash(inputMovement);
            Destroy(arrowPointerInstantiated);
            return;
            //Destroy(arrowPointerInstantiated);
            //state = State.Normal;
        }


    }
    public void PowerDash(Vector2 powerDashDirection)
    {
        
        canShieldAgainTimer = shieldAgainThreshold;
        canShieldAgainTimerLeft = shieldAgainThreshold;
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
        if (controlsMenuInstantiated != null)
        {
            controlsMenuInstantiated.transform.position = this.transform.position;
        }
    }
    public void HandleShockGrabbed()
    {
        if (isGrabbed)
        {
            rb.velocity = Vector3.zero;
        }
    }

    protected void HandleAirShielding()
    {

        canAirShieldTimer += Time.deltaTime;
        airShieldTimer -= Time.deltaTime;
        if (airShieldTimer > 0 && canAirShield && canAirShieldTimer > canAirShieldThreshold)
        {
            pressedAirShield = false;
            airShieldTimer = 0;
            airShielding = true;
            airShieldInstantiated = Instantiate(airShieldAnimation, transform.position, Quaternion.identity);
            canAirShield = false;
            airPowerShieldTimer = 0f;
            canAirShieldTimer = 0f;
        }
        
        if (airShielding)
        {
            if (airShieldInstantiated != null)
            {
                airShieldInstantiated.transform.position = this.transform.position;
            }
            airPowerShieldTimer += Time.deltaTime;
            if (airPowerShieldTimer >= .5f)
            {
                shieldingLeft = false;
                shieldingRight = false;
                airShielding = false;
                pressedAirShield = false;
                isBlockingLeft = false;
                isBlockingRight = false;
            }
            if (perfectShieldTimer >= .5f)
            {
                shieldLeftTimer = 0;
                shieldRightTimer = 0;
                shieldingLeft = false;
                shieldingRight = false;
            }
        }
    }


    #region InputRegion
    void OnMove(InputValue value)
    {
        inputMovement = value.Get<Vector2>();
    }
    public virtual void OnRightStick(InputValue value) //this actually dashes based on right stick input
    {
        if (value != null)
        {
            lookPositionRightStick = value.Get<Vector2>();
        }
        if (lookPositionRightStick.magnitude >= .5f)
        {
            joystickLook = lookPositionRightStick;
        }
        FaceJoystick();


    }

    void OnMouseMove(InputValue value)
    {

        mousePosition = value.Get<Vector2>();
        FaceMouse();
    }
    void OnLeftJoystickLook(InputValue value)
    {
        //Debug.Log(joystickLook);
        if (lookPositionRightStick.magnitude == 0)
        {
            joystickLook = value.Get<Vector2>();
        }
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
        pressedRight = true;
        releasedRight = false;
        
    }
    public virtual void OnPunchLeft()
    {
        pressedLeft = true;
        releasedLeft = false;
    }
    public virtual void OnReleasePunchRight()
    {
        pressedRight = false;
        pummeledRight = false;
        releasedRight = true;
    }
    public virtual void OnReleasePunchLeft()
    {
        pressedLeft = false;
        pummeledLeft = false;
        releasedLeft = true;
    }

    void OnShieldRight()
    {
        if (state == State.FireGrabbed) return;
        if (state == State.Grabbed) return;
        if (state == State.Dashing) return;
        if (state == State.Knockback)
        {
            pressedAirShield = true;
            airShieldTimer = inputBuffer;
        }
        if (!canAirShield && state == State.Knockback) return;
        if (state == State.Knockback && canAirShieldTimer <= canAirShieldThreshold)
        {
            return;
        }
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
        if (state == State.Knockback)
        {
            pressedAirShield = true;
            airShieldTimer = inputBuffer;
        }
        if (!canAirShield && state == State.Knockback) return;
        if (state == State.Knockback && canAirShieldTimer <= canAirShieldThreshold)
        {
            return;
        }
        shieldLeftTimer = inputBuffer;

        //shieldingLeft = true;
    }

    void OnReleaseShieldRight()
    {
        if (state == State.Knockback)
        {
            return;
        }
        shieldRightTimer = 0;
        shieldingRight = false;
    }
    void OnReleaseShieldLeft()
    {
        if (state == State.Knockback)
        {
            return;
        }
        shieldLeftTimer = 0;
        shieldingLeft = false;
    }



    protected virtual void OnMouseDash()
    {
        if (state != State.Normal) return;
        shieldingLeft = false;
        shieldingRight = false;
        isBlockingLeft = false;
        isBlockingRight = false;
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        //Vector2 direction = inputMovement.normalized;
        Dash(direction.normalized);


    }

    void OnShieldBoth()
    {
        pressedShieldBoth = true;
        
    }

    void OnReleaseShieldBoth()
    {
       
        releasedShieldBoth = true;
            
        
    }

    protected virtual void OnDash()
    {
        if (state != State.Normal) return;


        Dash(inputMovement.normalized);


    }
    void OnRestart()
    {
        if (gameManager.gameIsOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    void OnCharacterSelect()
    {
        SceneManager.LoadScene(0);
    }

    void OnStart()
    {
        Debug.Log("Pressed start");
        controlsMenuInstantiated = Instantiate(controlsMenu, transform.position, Quaternion.identity);
    }
    void OnReleaseStart()
    {
        if (controlsMenuInstantiated != null)
        {
            Destroy(controlsMenuInstantiated);
        }
    }
    #endregion

    protected virtual void CheckForInputs()
    {
        CheckForPunchRight();
        CheckForPunchLeft();
        CheckForShieldBoth();
        CheckForReleaseShieldBoth();
        
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
            pummeledRight = true;
        }
        if (shieldingLeft || shieldingRight || isBlockingLeft || isBlockingRight) return;
        if (state == State.Grabbing) return;
        if (state == State.FireGrabbed) return;
        if (state == State.Stunned) return;
        if (state == State.Dashing) return;
        if (state == State.Knockback && canAirShieldTimer < canAirShieldThreshold) return;
        if (shieldingLeft && state != State.PowerShielding || shieldingRight && state != State.PowerShielding) return;
        if (returningRight) return;
        
        //punchedRight = true;
        shieldingRight = false;


        if (punchedRightTimer > 0)
        {
            Vector2 joystickPosition = joystickLook.normalized;
            if (joystickPosition.x != 0 || joystickPosition.y != 0)
            {
                Vector2 lastLookedPosition = joystickPosition;
                //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
                transform.right = lastLookedPosition;
            }
            punchedRight = true;
            punchedRightTimer = 0;
        }
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
            pummeledLeft = true;
        }
        if (shieldingLeft || shieldingRight || isBlockingLeft || isBlockingRight) return;
        if (state == State.Grabbing) return;
        if (state == State.FireGrabbed) return;
        if (state == State.Stunned) return;
        if (state == State.Dashing) return;
        if (state == State.Knockback && canAirShieldTimer < canAirShieldThreshold) return;
        if (shieldingLeft && state != State.PowerShielding || shieldingRight && state != State.PowerShielding) return;
        if (returningLeft) return;
        
        //punchedLeft = true;
        shieldingLeft = false;



        if (punchedLeftTimer > 0)
        {
            punchedLeft = true;
            punchedLeftTimer = 0;
            Vector2 joystickPosition = joystickLook.normalized;
            if (joystickPosition.x != 0 || joystickPosition.y != 0)
            {
                Vector2 lastLookedPosition = joystickPosition;
                //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
                transform.right = lastLookedPosition;
            }
        }
        

    }

    protected void CheckForShieldBoth()
    {

        if (pressedShieldBoth)
        {
            if (state == State.FireGrabbed) return;
            if (state == State.ShockGrabbed) return;
            if (state == State.Grabbed) return;
            if (state == State.Dashing) return;
            if (punchedRight || punchedLeft || returningRight || returningLeft) return;
            if (!canAirShield && state == State.Knockback) return;
            if (shieldingLeft || shieldingRight || isBlockingRight || isBlockingLeft) return;
            if (state == State.Knockback && canAirShieldTimer <= canAirShieldThreshold)
            {
                return;
            }

            if (punchedRight && punchedLeft)
            {
                return;
            }
            if (state == State.Knockback)
            {
                pressedAirShield = true;
                airShieldTimer = inputBuffer;
            }
            shieldLeftTimer = inputBuffer;
            shieldRightTimer = inputBuffer;
            Debug.Log("Pressed Shield");
            pressedShieldBoth = false;
        }
    }

    protected void CheckForReleaseShieldBoth()
    {
        if (releasedShieldBoth)
        {
            pressedShieldBoth = false;
            Debug.Log("release shield");
            if (state == State.Knockback)
            {
                releaseShieldBuffer = true;
                return;
            }
            shieldLeftTimer = 0;
            shieldRightTimer = 0;
            shieldingLeft = false;
            shieldingRight = false;

            releasedShieldBoth = false;
        }
    }
}
