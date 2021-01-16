using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : PlayerController
{
    public GameObject hammerLeft;
    public GameObject hammerRight;
    
    public Rigidbody2D thrownRightHammerRB, thrownLeftHammerRB;
    public GameObject thrownRightHammer;
    public GameObject thrownLeftHammer;
    public GameObject thrownHammerPrefab;
    public bool threwRight, returnHammerRight, threwLeft, returnHammerLeft = false;
    public float hammerSpeed, returnHammerRightSpeed, returnLeftHammerSpeed = 50f;
    public Vector2 oppositeRightHammerForce, oppositeLeftHammerForce;
    public float dashTimer;
    public GameObject lightningPrefab, lightningBallPrefab, instantiatedLightning, instantiatedLightningBall;
    public Rigidbody2D instantiatedLightningBallRB;
    public override void Start()
    {
        animationTransformHandler = Instantiate(playerAnimatorBase, transform.position, Quaternion.identity).GetComponent<AnimationTransformHandler>();
        animationTransformHandler.SetPlayer(this.gameObject);
        animator = animationTransformHandler.GetComponent<Animator>();
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

    public override void HandleThrowingHands()
    {
        
        punchedRightTimer -= Time.deltaTime;
        punchedLeftTimer -= Time.deltaTime;
        if (punchedLeftTimer > 0 && leftHandTransform.localPosition.x <= 0) punchedLeft = true;
        if (punchedRightTimer > 0 && rightHandTransform.localPosition.x <= 0) punchedRight = true;


        //Debug.Log(returningRight); current problem is when i am thrown returning right is set to true and its not set back again 
        if (punchedRight && returningRight == false && returnHammerRight == false)
        {
            punchedRightTimer = 0;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(punchRange, .6f), punchSpeed * Time.deltaTime);
            if (rightHandTransform.localPosition.x == punchRange && threwRight == false && returnHammerRight == false)
            {
                returningRight = true;
                hammerRight.SetActive(false);
                threwRight = true;
                thrownRightHammer = Instantiate(thrownHammerPrefab, rightHandTransform.position, transform.rotation);
                thrownRightHammer.GetComponent<ThrownHammer>().SetPlayer(this, 0);
                thrownRightHammerRB = thrownRightHammer.GetComponent<Rigidbody2D>();
                thrownRightHammerRB.AddForce(transform.right * hammerSpeed, ForceMode2D.Impulse);
            }
        }

        if (returnHammerRight)
        {

            if (thrownRightHammerRB.velocity.magnitude > .2f)
            {
                thrownRightHammerRB.AddForce(oppositeRightHammerForce * Time.deltaTime * 5, ForceMode2D.Impulse);
            }
            if (thrownRightHammerRB.velocity.magnitude <= 1f)
            {
                thrownRightHammerRB.velocity = Vector3.zero;

                returnHammerRightSpeed += 200 * Time.deltaTime;
                thrownRightHammerRB.transform.position = Vector3.MoveTowards(thrownRightHammerRB.transform.position, rightHandTransform.position, returnHammerRightSpeed * Time.deltaTime);
            }
            
            
        }

        if (returningRight && returnHammerRight == true)
        {
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(0, 0), returnHammerRightSpeed * Time.deltaTime);
            punchedRight = false;
            
            if (rightHandTransform.localPosition.x <= 0f && thrownRightHammerRB.transform.position == rightHandTransform.position)
            {
                Destroy(thrownRightHammer);
                hammerRight.SetActive(true);
                returnHammerRight = false;
                returningRight = false;
                threwRight = false;
                returnHammerRight = false;
            }
        }

        if (rightHandTransform.localPosition.x <= 0 && !returnHammerRight)
        {
            returningRight = false;
        }
        


        if (punchedLeft && returningLeft == false && instantiatedLightningBall == null)
        {
            punchedLeftTimer = 0;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(punchRange, -.7f), punchSpeed * Time.deltaTime);
            if (leftHandTransform.localPosition.x >= punchRange && returningLeft == false)
            {
                instantiatedLightningBall = Instantiate(lightningBallPrefab, leftHandTransform.position, transform.rotation);
                instantiatedLightningBall.GetComponent<LightningBall>().SetPlayer(this);
                instantiatedLightningBallRB = instantiatedLightningBall.GetComponent<Rigidbody2D>();
                instantiatedLightningBallRB.AddForce(transform.right * 10, ForceMode2D.Impulse);
                returningLeft = true;
            }
        }
        if (returningLeft)
        {
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
            punchedLeft = false;
            if (leftHandTransform.localPosition.x <= 0f)
            {
                returningLeft = false;
            }
        }

        /*if (punchedLeft && returningLeft == false && returnHammerLeft == false)
        {
            punchedLeftTimer = 0;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(punchRange, -.6f), punchSpeed * Time.deltaTime);
            if (leftHandTransform.localPosition.x == punchRange && threwLeft == false && returnHammerLeft == false)
            {
                hammerLeft.SetActive(false);
                threwLeft = true;
                thrownLeftHammer = Instantiate(thrownHammerPrefab, leftHandTransform.position, transform.rotation);
                thrownLeftHammer.GetComponent<ThrownHammer>().SetPlayer(this, 1);
                thrownLeftHammerRB = thrownLeftHammer.GetComponent<Rigidbody2D>();
                thrownLeftHammerRB.AddForce(transform.right * hammerSpeed, ForceMode2D.Impulse);
                returningLeft = true;
            }
        }

        if (returnHammerLeft)
        {
            if (thrownLeftHammerRB.velocity.magnitude > .2f)
            {
                thrownLeftHammerRB.AddForce(oppositeLeftHammerForce * Time.deltaTime * 5, ForceMode2D.Impulse);
            }
            if (thrownLeftHammerRB.velocity.magnitude <= 1f)
            {
                thrownLeftHammerRB.velocity = Vector3.zero;
                returnLeftHammerSpeed += 200 * Time.deltaTime;
                thrownLeftHammer.transform.position = Vector3.MoveTowards(thrownLeftHammerRB.transform.position, leftHandTransform.position, returnLeftHammerSpeed * Time.deltaTime);
            }
            

        }

        if (returningLeft && returnHammerLeft == true)
        {

            punchedLeft = false;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(0, 0), returnLeftHammerSpeed * Time.deltaTime);
            if (leftHandTransform.localPosition.x <= 0f && thrownLeftHammerRB.transform.position == leftHandTransform.position)
            {
                hammerLeft.SetActive(true);
                returnHammerLeft = false;
                Destroy(thrownLeftHammer);

                returningLeft = false;
                threwLeft = false;
                returnHammerLeft = false;
            }
        }*/
        dashTimer += Time.deltaTime;
    }

    public void ReturnRightHammer()
    {
        
        thrownRightHammerRB.velocity = Vector3.zero;
        returnHammerRightSpeed = 5f;
        returnHammerRight = true;
        punchedRight = false;
    }
    public void ReturnLeftHammer()
    {
        thrownLeftHammerRB.velocity = Vector3.zero;
        returnLeftHammerSpeed = 5f;
        returnHammerLeft = true;
        punchedLeft = false;
    }


    public override void HandleShielding()
    {
        if (punchedRight || punchedLeft) return;
        shieldLeftTimer -= Time.deltaTime;
        if (shieldLeftTimer > 0 && returningLeft == false && punchedLeft == false)
        {
            shieldingLeft = true;
        }
        shieldRightTimer -= Time.deltaTime;
        if (shieldRightTimer > 0 && returningRight == false && punchedRight == false)
        {
            shieldingRight = true;
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
            rightHandTransform.localScale = new Vector2(1, 1);
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
            leftHandTransform.localScale = new Vector2(1, 1);
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
        }
        if (rightHandTransform.localPosition.x == 0 && rightHandTransform.localPosition.y == .4f)
        {
            isBlockingRight = true;

        }
        if (rightHandTransform.localPosition.y != .4f)
        {
            isBlockingRight = false;
        }
        if (leftHandTransform.localPosition.y != -.4f)
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
            moveSpeed = 8f;
        }


    }

    public override void Dash(Vector3 direction)
    {
        
            dashPosition = direction;
            transform.right = dashPosition;
            if (!punchedRight && !returningRight && !returnHammerRight)
            {
                state = State.Dashing;
            }
       
        
    }

    public override void HandleDash()
    {

        transform.right = dashPosition;
        returnSpeed = 1;
        rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(punchRange, .6f), punchSpeed * Time.deltaTime);
        if (rightHandTransform.localPosition.x == punchRange && threwRight == false && returnHammerRight == false)
        {
            hammerRight.SetActive(false);
            threwRight = true;
            thrownRightHammer = Instantiate(thrownHammerPrefab, rightHandTransform.position, transform.rotation);
            thrownRightHammer.GetComponent<ThrownHammer>().SetPlayer(this, 0);
            thrownRightHammerRB = thrownRightHammer.GetComponent<Rigidbody2D>();
            thrownRightHammerRB.AddForce(transform.right * 30f, ForceMode2D.Impulse);
        }
        if (thrownRightHammerRB != null)
        {

            rb.velocity = thrownRightHammerRB.velocity;
        }
        if (thrownRightHammerRB == null)
        {
            rb.velocity = Vector3.zero;
        }
        
        if (returnHammerRight)
        {


            thrownRightHammerRB.velocity = Vector3.zero;
            if (thrownRightHammerRB.velocity.magnitude > .2f)
            {
                thrownRightHammerRB.AddForce(oppositeRightHammerForce * .005f, ForceMode2D.Impulse);
            }
            if (thrownRightHammerRB.velocity.magnitude <= .2f)
            {

                returnHammerRightSpeed += 200 * Time.deltaTime;
                thrownRightHammerRB.transform.position = Vector3.MoveTowards(thrownRightHammerRB.transform.position, rightHandTransform.position, returnSpeed * Time.deltaTime);
            }
            if (thrownRightHammerRB.transform.position == rightHandTransform.position)
            {
                returningRight = true;
            }


        }
        if (returningRight)
        {

            thrownRightHammerRB.velocity = Vector3.zero;
            thrownRightHammerRB.transform.position = Vector3.MoveTowards(thrownRightHammerRB.transform.position, rightHandTransform.position, returnHammerRightSpeed * Time.deltaTime);
            threwRight = false;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector2(0, 0), returnHammerRightSpeed * Time.deltaTime);
            punchedRight = false;
            
            
            if (rightHandTransform.localPosition.x <= 0f)
            {
                thrownRightHammerRB.velocity = Vector3.zero;
                dashTimer = 0;
                returnHammerRight = false;
                returningRight = false;
                Destroy(thrownRightHammer);
                state = State.Normal;
                hammerRight.SetActive(true);
            }
        }


        /*leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(punchRange, .6f), punchSpeed * Time.deltaTime);
        if (leftHandTransform.localPosition.x == punchRange && threwLeft == false && returnHammerLeft == false)
        {
            hammerLeft.SetActive(false);
            threwLeft = true;
            thrownLeftHammer = Instantiate(thrownHammerPrefab, leftHandTransform.position, transform.rotation);
            thrownLeftHammer.GetComponent<ThrownHammer>().SetPlayer(this, 0);
            thrownLeftHammerRB = thrownLeftHammer.GetComponent<Rigidbody2D>();
            thrownLeftHammerRB.AddForce(transform.right * hammerSpeed, ForceMode2D.Impulse);
        }
        if (thrownLeftHammerRB != null)
        {

            rb.velocity = thrownLeftHammerRB.velocity;
        }
        if (thrownLeftHammerRB == null)
        {
            rb.velocity = Vector3.zero;
        }

        if (returnHammerLeft)
        {


            if (thrownLeftHammerRB.velocity.magnitude > .2f)
            {
                thrownLeftHammerRB.AddForce(oppositeRightHammerForce * .005f, ForceMode2D.Impulse);
            }
            if (thrownLeftHammerRB.velocity.magnitude <= .2f)
            {

                returnLeftHammerSpeed += 200 * Time.deltaTime;
                thrownLeftHammerRB.transform.position = Vector3.MoveTowards(thrownLeftHammerRB.transform.position, leftHandTransform.position, returnLeftHammerSpeed * Time.deltaTime);
            }
            if (thrownLeftHammerRB.transform.position == leftHandTransform.position)
            {
                returningLeft = true;
            }


        }
        if (returningLeft)
        {

            thrownLeftHammerRB.transform.position = Vector3.MoveTowards(thrownLeftHammerRB.transform.position, leftHandTransform.position, returnLeftHammerSpeed * Time.deltaTime);
            threwLeft = false;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector2(0, 0), returnLeftHammerSpeed * Time.deltaTime);
            punchedLeft = false;


            if (rightHandTransform.localPosition.x <= 0f)
            {

                returnHammerLeft = false;
                returningLeft = false;
                Destroy(thrownLeftHammer);
                state = State.Normal;
                hammerLeft.SetActive(true);
            }
        }*/
    }


    public override void EndPunchRight()
    {
        if (thrownRightHammer != null)
        {
            threwRight = false;
            returnHammerRight = true;
        }
        returningRight = true;
        punchedRight = false;
        punchedRightTimer = 0f;
        rightHandCollider.enabled = false;
        rightHandTransform.localScale = new Vector2(1, 1);
        pummeledLeft = false;
        //state = State.Normal;
    }

    public override void EndPunchLeft()
    {
        threwLeft = true;
        returningLeft = true;
        punchedLeft = false;
        punchedLeftTimer = 0f;
        leftHandCollider.enabled = false;
        leftHandTransform.localScale = new Vector2(1, 1);
        pummeledRight = false;
        //state = State.Normal;
    }




    public override void FaceMouse()
    {
        
        if (state == State.PowerShieldStunned) return;
        if (state == State.Dashing) return;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.right = direction;
    }

    public override void OnReleasePunchRight()
    {
        punchedRightTimer = 0;
        if (threwRight && returnHammerRight == false)
        {
            Debug.Log("release");
            oppositeRightHammerForce = -thrownRightHammerRB.velocity;
            returnHammerRightSpeed = 0f;
            returnHammerRight = true;
            
        }
        if (punchedRight)
        {
            returningRight = true;
            threwRight = false;
            punchedRight = false;
        }
    }
    public override void OnReleasePunchLeft()
    {
        
    }

    public override void OnPunchRight()
    {
        if (state == State.Grabbing) return;
        if (state == State.Stunned) return;
        if (state == State.Dashing) return;
        //if (state == State.Knockback) return;
        punchedRightTimer = inputBuffer;
        if (returningRight) return;
        //punchedRight = true;
        shieldingRight = false;
    }
    public override void OnPunchLeft()
    {
        if (state == State.Grabbing) return;
        if (state == State.Stunned) return;
        if (state == State.Dashing) return;
        //if (state == State.Knockback) return;
        punchedLeftTimer = inputBuffer;
        if (returningLeft) return;
        //punchedLeft = true;
        shieldingLeft = false;
    }
    public override void FaceJoystick()
    {
        
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
    void OnReleaseDash()
    {
        if (punchedRight)
        {
            state = State.Normal;
        }
        if (state == State.Dashing && thrownRightHammer != null)
        {

            oppositeRightHammerForce = -thrownRightHammerRB.velocity;
            returnHammerRight = true;
        }

        //state = State.Normal;
    }
    void OnReleaseDashController()
    {
        if (state == State.Dashing && thrownRightHammer == null)
        {
            returningRight = true;
            threwRight = false;
            punchedRight = false;
            state = State.Normal;
        }
        if (state == State.Dashing && thrownRightHammer != null)
        {

            oppositeRightHammerForce = -thrownRightHammerRB.velocity;
            returnHammerRight = true;
        }

        //state = State.Normal;
    }
}
