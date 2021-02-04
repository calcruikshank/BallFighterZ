using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaCS : PlayerController
{
    [SerializeField] GameObject shurikenPrefab;
    [SerializeField] GameObject kunaiPrefab;
    [SerializeField] GameObject sliceAnimation;
    public CircleCollider2D hitBox;
    Collider2D opponentHitBox;
    [SerializeField] Transform midFire, botFire, topFire;
    Rigidbody2D midFireRB, botFireRB, topFireRB, kunaiRB;
    Vector3 dashDir;
    float dashSpeedNinja;
    float shurikenSpeed = 30f;
    // Start is called before the first frame update
    public override void Start()
    {
        canAirShieldThreshold = .1f;
        if (playerAnimatorBase != null)
        {
            animationTransformHandler = Instantiate(playerAnimatorBase, transform.position, Quaternion.identity).GetComponent<AnimationTransformHandler>();
            animationTransformHandler.SetPlayer(this.gameObject);
            animator = animationTransformHandler.GetComponent<Animator>();
        }

        totalShieldRemaining = 225f / 255f;
        gameManager.SetTeam((PlayerController)this);
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



    public override void HandleThrowingHands()
    {
        
        if (state != State.Dashing)
        {
            hitBox.enabled = true;
            hitBox.isTrigger = false;
            if (opponentHitBox != null)
            {
                Physics2D.IgnoreCollision(hitBox, opponentHitBox, false);
            }
        }
        
        punchedLeftTimer -= Time.deltaTime;
        if (punchedLeftTimer > 0) punchedLeft = true;

        if (punchedRight && !returningRight && !punchedLeft)
        {
            punchedRightTimer = 0;
            punchedLeftTimer = 0;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(punchRange, -.7f), punchSpeed * Time.deltaTime);
            if (rightHandTransform.localPosition.x == punchRange)
            {
                ThrowShuriken();
                returningRight = true;
                punchedRight = false;
                punchedLeft = false;
            }
        }

        if (returningRight)
        {
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
            
            if (rightHandTransform.localPosition.x <= 0f)
            {
                returningRight = false;
            }
        }

        if (punchedLeft && !returningRight && !punchedRight)
        {
            punchedLeftTimer = 0;
            punchedRightTimer = 0;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(punchRange, -.7f), punchSpeed * Time.deltaTime);
            if (rightHandTransform.localPosition.x == punchRange)
            {
                ThrowKunai();
                returningRight = true;
                punchedLeft = false;
                punchedRight = false;
            }
        }






        returningLeft = false;
        if (!returningLeft || !returningRight && state != State.Dashing) returnSpeed = 2.5f;
        

    }

    void ThrowShuriken()
    {
        GameObject newShurikenMid = Instantiate(shurikenPrefab, midFire.position, midFire.rotation);
        GameObject newShurikenTop = Instantiate(shurikenPrefab, topFire.position, topFire.rotation);
        GameObject newShurikenBot = Instantiate(shurikenPrefab, botFire.position, botFire.rotation);
        botFireRB = newShurikenBot.GetComponent<Rigidbody2D>();
        topFireRB = newShurikenTop.GetComponent<Rigidbody2D>();
        midFireRB = newShurikenMid.GetComponent<Rigidbody2D>();
        midFireRB.AddForce(midFire.right * shurikenSpeed, ForceMode2D.Impulse);
        topFireRB.AddForce(topFire.right * shurikenSpeed, ForceMode2D.Impulse);
        botFireRB.AddForce(botFire.right * shurikenSpeed, ForceMode2D.Impulse);
        newShurikenBot.GetComponent<Shuriken>().SetPlayer(this);
        newShurikenTop.GetComponent<Shuriken>().SetPlayer(this);
        newShurikenMid.GetComponent<Shuriken>().SetPlayer(this);
        
    }
    void ThrowKunai()
    {
        GameObject newKunai = Instantiate(kunaiPrefab, midFire.position, midFire.rotation);
        kunaiRB = newKunai.GetComponent<Rigidbody2D>();
        kunaiRB.AddForce(midFire.right * shurikenSpeed / 2, ForceMode2D.Impulse);
        newKunai.GetComponent<Kunai>().SetPlayer(this);
    }


    public override void Dash(Vector3 direction)
    {
        shieldingLeft = false;
        shieldingRight = false;
        isBlockingLeft = false;
        isBlockingRight = false;
        hitBox.isTrigger = true;
        Debug.Log("dash");
        dashSpeedNinja = 80f;
        dashDir = direction;
        state = State.Dashing;
        Debug.Log(direction);
        this.transform.GetComponent<CircleCollider2D>().enabled = false;
    }

    public override void HandleDash()
    {
        
        Debug.Log("dashing State " + state);
        float powerDashSpeedMulti = 8f;
        dashSpeedNinja -= dashSpeedNinja * powerDashSpeedMulti * Time.deltaTime;

        float powerDashMinSpeed = 1f;
        Debug.Log(powerDashSpeed);
        if (dashSpeedNinja < powerDashMinSpeed)
        {
            if (opponentHitBox != null)
            {
                Physics2D.IgnoreCollision(hitBox, opponentHitBox, false);
            }
            hitBox.enabled = true;
            hitBox.isTrigger = false;
            state = State.Normal;
        }
    }
    public override void FixedHandleDash()
    {
        rb.velocity = dashDir * dashSpeedNinja;
    }

    public override void HandleKnockback()
    {

        hitBox.enabled = true;
        hitBox.isTrigger = false;
        if (opponentHitBox != null)
        {
            Physics2D.IgnoreCollision(hitBox, opponentHitBox, false);
        }
        canAirShieldTimer += Time.deltaTime;
        
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
            if (releaseShieldBuffer)
            {
                releaseShieldBuffer = false;
                shieldLeftTimer = 0;
                shieldRightTimer = 0;
                shieldingLeft = false;
                shieldingRight = false;
            }
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

    public override void OnPunchLeft()
    {
        if (state == State.Grabbing) return;
        if (state == State.FireGrabbed) return;
        if (state == State.Stunned) return;
        if (state == State.Dashing) return;
        if (state == State.Knockback && canAirShieldTimer < canAirShieldThreshold) return;
        if (shieldingLeft && state != State.PowerShielding || shieldingRight && state != State.PowerShielding) return;
        if (punchedRight) return;
        //if (state == State.Knockback) return;
        Vector2 joystickPosition = joystickLook.normalized;
        if (joystickPosition.x != 0 || joystickPosition.y != 0)
        {
            Vector2 lastLookedPosition = joystickPosition;
            //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            transform.right = lastLookedPosition;
        }
        punchedRightTimer = 0f;
        punchedLeftTimer = inputBuffer;
        if (returningLeft) return;
        //punchedLeft = true;
        shieldingLeft = false;
    }

    

    protected override void OnMouseDash()
    {
        if (state != State.Normal)
        {
            return;
        }

        Dash(transform.right.normalized);
    }
    protected override void OnDash()
    {
        if (state != State.Normal)
        {
            return;
        }

        Dash(transform.right.normalized);
    }


    protected override void CheckForPunchRight()
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
        if (punchedLeft) return;
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



    void OnTriggerEnter2D(Collider2D other)
    {
        if (state != State.Dashing)
        {
            return;
        }
        opponent = other.transform.parent.GetComponent<PlayerController>();
        Environment environment = other.transform.GetComponent<Environment>();
        if (environment != null)
        {
            hitBox.isTrigger = false;
            hitBox.enabled = true;
        }
        if (opponent != null && opponent != this)
        {
            Instantiate(sliceAnimation, opponent.transform.position, transform.rotation);
            opponentHitBox = other;
            Physics2D.IgnoreCollision(hitBox, opponentHitBox);
            if (opponent.isBlockingLeft || opponent.isBlockingRight)
            {
                if (opponent.isPowerShielding)
                {
                    opponent.totalShieldRemaining += 20f / 255f;
                    opponent.PowerShield();
                    this.PowerShieldStun();
                    return;

                }
                opponent.totalShieldRemaining -= 10f / 255f;
                return;
            }
            if (opponent != null && opponent != this)
            {
                opponent.rb.velocity = Vector3.zero;
                opponent.Knockback(5, -transform.right);
            }


        }

    }



}
