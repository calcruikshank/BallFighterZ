﻿using System.Collections;
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
    [SerializeField] GameObject fireFoxPrefab;
    [SerializeField] CircleCollider2D bodyCollider;
    GameObject leftHandFire, rightHandFire;
    float beginDashTimer = .25f;
    float ultTimer = 0f;
    public override void Start()
    {

        canAirShieldThreshold = .2f;
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
        if (comboMeterScript == null)
        {
            comboMeterScript = comboMeter.GetComponent<ComboMeter>();
        }
        rightHandCollider = rightHandTransform.GetComponent<CircleCollider2D>();
        leftHandCollider = leftHandTransform.GetComponent<CircleCollider2D>();
        canDash = true;
        stocksLeft = 4;
        stocksLeftText.text = (stocksLeft.ToString());
        dashTimer = 3;

    }

    public override void HandleMovement()
    {
       
        movement.x = inputMovement.x;
        movement.y = inputMovement.y;
        if (movement.x != 0 || movement.y != 0)
        {
            lastMoveDir = movement;
        }
        stunnedTimer = 0;
        isGrabbed = false;
        dashedTimer = 0f;
        canDash = true;

        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(movement.y));
            animator.SetFloat("SpeedX", Mathf.Abs(movement.x));
        }
        respawned = false;
        dashingIdle = false;

        animationTransformHandler.SetEmittingToFalse();
    }
    public override void HandleThrowingHands()
    {
        if (animator != null)
        {
            animator.SetBool("punchedRight", (punchedRight));
            animator.SetBool("punchedLeft", (punchedLeft));
            animator.SetBool("returningRight", (returningRight));
            animator.SetBool("returningLeft", (returningLeft));
            animator.SetFloat("rightHandPosition", (rightHandTransform.localPosition.x));
            animator.SetFloat("leftHandPosition", (leftHandTransform.localPosition.x));
        }
        if (!returningLeft || !returningRight) returnSpeed = 4f;
        if (returningLeft && leftHandTransform.localPosition.x <= 1.25f && returningRight && rightHandTransform.localPosition.x <= 1.25f && state != State.Dashing)
        {
            returnSpeed = 1f;
        }
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

        if (rightHandTransform.localPosition.x == punchRange && releasedRight && state != State.Dashing)
        {
            if (thrownRightFireball != null)
            {

                thrownRightFireball.GetComponent<Fireball>().DestroyRightFireball();

            }
            returningRight = true;
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

        if (leftHandTransform.localPosition.x == punchRange && releasedLeft && state != State.Dashing)
        {
            if (thrownLeftFireball != null)
            {
                thrownLeftFireball.GetComponent<Fireball>().DestroyLeftFireball();
            }
            returningLeft = true;
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
        /*if (thrownRightFireball == null && punchedRight == true)
        {
            GameObject instantiatedExplosion = Instantiate(explosionFireballPrefab, rightHandTransform.position, this.transform.rotation);
            instantiatedExplosion.GetComponent<ExplosionScript>().SetPlayer(this);
        }*/
        
        pressedRight = false;
        pummeledRight = false;
        releasedRight = true;
    }
    public override void OnAltReleaseRight()
    {

        pressedRight = false;
        pummeledRight = false;
        releasedRight = true;
    }
    public override void OnAltReleaseLeft()
    {
        pressedLeft = false;
        pummeledLeft = false;
        releasedLeft = true;
    }
    public override void OnReleasePunchLeft()
    {
        
        /*if (thrownLeftFireball == null && punchedLeft == true)
        {
            GameObject instantiatedExplosion = Instantiate(explosionFireballPrefab, leftHandTransform.position, this.transform.rotation);
            instantiatedExplosion.GetComponent<ExplosionScript>().SetPlayer(this);
        }*/
        pressedLeft = false;
        pummeledLeft = false;
        releasedLeft = true;
    }



    public override void HandleDash()
    {
        if (beginDashTimer > 0)
        {
            rb.velocity = Vector3.zero;
        }
        beginDashTimer -= Time.deltaTime;
        if (beginDashTimer <= 0)
        {
            //rightHandFire.transform.position = transform.position;
            //rightHandFire.transform.rotation = transform.rotation;
            //leftHandFire.transform.position = transform.position;
            //leftHandFire.transform.rotation = transform.rotation;
            //leftHandFire.transform.localScale = leftHandTransform.localScale;
            //rightHandFire.transform.localScale = leftHandTransform.localScale;
            dashLookPosition = Vector3.RotateTowards(transform.right, inputMovement.normalized, 8 * Time.deltaTime, 0f);

            //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            if (!dashingIdle)
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
                    Destroy(rightHandFire); 
                    Destroy(leftHandFire); 
                    rightStickLook = Vector3.zero;
                    dashingIdle = false;
                    state = State.Normal;
                }
            }
        }
        
    }

    public override void FixedHandleDash()
    {
        if (beginDashTimer <= 0)
        {
            rb.velocity = movement * dashSpeed;

            Time.timeScale = 1;
        }
        
    }

    protected override void UseUltimate()
    {
        isUlting = true;
        canUltimate = false;
        ultTimer = 3f;
        moveSpeed = 25f;
        meterCount = 0;
        comboMeterScript.SetMeter(meterCount);
        fireFoxPrefab.GetComponent<ParticleSystem>().Play();

        state = State.UltimateState;
    }
    protected override void HandleUltimate()
    {
        threwLeft = false;
        threwRight = false;
        
        
        ultTimer -= Time.deltaTime;
        if (ultTimer <= 0f)
        {

            fireFoxPrefab.GetComponent<ParticleSystem>().Stop();
            isUlting = false;
            state = State.Normal;
        }
        HandleMovement();
    }
    protected override void FixedHandleUltMovement()
    {
        FixedHandleMovement();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (state != State.UltimateState) return;
        opponent = other.transform.GetComponent<PlayerController>();
        if (opponent != null)
        {
            if (opponent.isPowerShielding)
            {
                opponent.totalShieldRemaining += 20f / 255f;
                opponent.PowerShield();
                opponent.rb.velocity = Vector3.zero;
                HitImpact(this.transform);
                return;
            }
            HitImpact(transform);
            Vector2 punchTowards = transform.right;
            opponent.rb.velocity = Vector3.zero;
            opponent.Knockback(10, punchTowards);

            //Physics2D.IgnoreCollision(this.transform.GetComponent<Collider2D>(), other);
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
        //rightHandFire = Instantiate(fireFoxPrefab, transform.position, transform.rotation);
        //leftHandFire = Instantiate(fireFoxPrefab, transform.position, transform.rotation);
        beginDashTimer = .25f;
        state = State.Dashing;

    }

    public override void Respawn()
    {
        comboCounter = 1;
        fireFoxPrefab.GetComponent<ParticleSystem>().Stop();
        isUlting = false;
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
    protected override void FaceLookDirection()
    {
        if (punchedLeft || punchedRight || leftHandTransform.localPosition.x > .1f && returningLeft || rightHandTransform.localPosition.x > .1f && returningRight) return;
        if (state == State.Dashing) return;
        if (state == State.PowerShieldStunned) return;
        Vector2 lookTowards = lookDirection.normalized;
        if (lookTowards.x != 0 || lookTowards.y != 0)
        {
            lastLookedPosition = lookTowards;

            //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        }

        Look();
    }

    protected override void Look()
    {
        transform.right = lastLookedPosition;

    }
    protected override void OnDash()
    {
        if (punchedLeft || punchedRight) return;
        if (state != State.Normal && state != State.Dashing) return;
        if (dashingIdle)
        {
            return;
        }
        rightStickLook = inputMovement;
        if (rightStickLook.magnitude >= .8f)
        {

            if (state == State.Dashing) return;
            transform.right = rightStickLook.normalized;
            returningRight = false;
            returningLeft = false;
            dashSpeed = 30f;
            dashingTimer = 0f;
            /*rightHandFire = Instantiate(fireFoxPrefab, rightHandTransform.position, rightHandTransform.rotation);
            leftHandFire = Instantiate(fireFoxPrefab, leftHandTransform.position, leftHandTransform.rotation);*/
            //rightHandFire = Instantiate(fireFoxPrefab, transform.position, transform.rotation);
            //leftHandFire = Instantiate(fireFoxPrefab, transform.position, transform.rotation);
            beginDashTimer = .25f;
            state = State.Dashing;
        }
    }
}
