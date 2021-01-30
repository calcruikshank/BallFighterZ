using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaCS : PlayerController
{
    [SerializeField] GameObject shurikenPrefab;
    [SerializeField] GameObject kunaiPrefab;
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
        if (state != State.Dashing && opponentHitBox != null)
        {
            hitBox.enabled = true;
            hitBox.isTrigger = false;
            Physics2D.IgnoreCollision(hitBox, opponentHitBox, false);
        }
        punchedRightTimer -= Time.deltaTime;
        punchedLeftTimer -= Time.deltaTime;
        if (punchedLeftTimer > 0) punchedLeft = true;
        if (punchedRightTimer > 0) punchedRight = true;

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
            punchedRight = false;
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
        if (!returningLeft || !returningRight && state != State.Dashing) returnSpeed = 4f;
        

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



    public override void OnPunchRight()
    {
        pummeledRight = true;
        if (state == State.Grabbing) return;
        if (state == State.FireGrabbed) return;
        if (state == State.Stunned) return;
        if (state == State.Dashing) return;
        if (state == State.Knockback && canAirShieldTimer < canAirShieldThreshold) return;
        if (shieldingLeft && state != State.PowerShielding || shieldingRight && state != State.PowerShielding) return;
        if (punchedLeft) return;
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
    public override void OnPunchLeft()
    {
        pummeledLeft = true;
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
        punchedLeftTimer = inputBuffer;
        if (returningLeft) return;
        //punchedLeft = true;
        shieldingLeft = false;
    }

    public override void OnReleasePunchRight()
    {

    }
    public override void OnReleasePunchLeft()
    {

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
