using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlayer : PlayerController
{
    [SerializeField] GameObject fireballInHand;
    [SerializeField] GameObject fireballPrefab, explosionPrefab, bigFireball;
    GameObject fireballInstantiated, fireballInstantiatedLeft, bigInstantiated;
    bool spawnedLeft, spawnedRight, spawnedBigFireball = false;
    float fireballSpeed = 60f;
    
    protected override void Update()
    {
        switch (state)
        {
            case State.Normal:
                HandleMovement();
                HandleThrowingHands();
                HandleShielding();
                HandleAButton();
                break;
            case State.Knockback:
                HandleKnockback();
                HandleThrowingHands();
                HandleShielding();
                break;
            case State.ParryState:
                HandleParry();
                HandleShielding();
                break;
            case State.PowerDashing:
                HandlePowerDashing();
                HandleShielding();
                HandleThrowingHands();
                HandleAButton();
                break;
            case State.WaveDahsing:
                HandleWaveDashing();
                HandleShielding();
                HandleThrowingHands();
                break;
            case State.ParryStunned:
                HandleShielding();
                HandleParryStunned();
                break;
            case State.Dashing:
                HandleDash();
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
            case State.Stunned:
                HandleStunned();
                HandleThrowingHands();
                HandleShielding();
                break;
        }

        CheckForInputs();
        FaceLookDirection();
    }

    protected override void FixedUpdate()
    {

        switch (state)
        {
            case State.Normal:
                FixedHandleMovement();
                break;
            case State.PowerDashing:
                FixedHandlePowerDashing();
                break;
            case State.WaveDahsing:
                FixedHandleWaveDashing();
                break;
            case State.Dashing:
                //FixedHandleMovement();
                break;
        }
    }
    protected override void HandleThrowingHands()
    {
       
            animatorUpdated.SetBool("punchingLeft", (punchedLeft));
            animatorUpdated.SetBool("punchingRight", (punchedRight));

        #region GrabRegion
        if (punchedLeft && !releasedRight && grabbing == false || punchedRight && !releasedLeft && grabbing == false)
        {
            spawnedBigFireball = false;
            animatorUpdated.SetBool("Grabbing", true);
            leftHandTransform.localPosition = Vector3.zero;
            rightHandTransform.localPosition = Vector3.zero;
            
            grabbing = true;
           
        }

        if (grabbing)
        {
            rb.velocity = Vector3.zero;
            moveSpeed = 0f;
        }
        if (grabbing)
        {
            rb.velocity = Vector3.zero;
            moveSpeed = 0f;
            if (!returningRight)
            {
                leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector3(punchRange, -.4f, -.4f), punchSpeed * .125f * Time.deltaTime);
                rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(punchRange, -.4f, .4f), punchSpeed * .125f * Time.deltaTime);
            }
            if (rightHandTransform.localPosition.x >= punchRange / 2 && spawnedBigFireball == false)
            {
                spawnedBigFireball = true;
                bigInstantiated = Instantiate(bigFireball, GrabPosition.position, Quaternion.identity);
                bigInstantiated.GetComponent<Rigidbody>().velocity = Vector3.zero;
                bigInstantiated.GetComponent<HandleColliderShieldBreak>().SetPlayer(this, rightHandTransform);
            }
            if (rightHandTransform.localPosition.x >= punchRange / 2 && rightHandTransform.localPosition.x < punchRange * .9f && bigInstantiated.GetComponent<Rigidbody>().velocity == Vector3.zero)
            {
                bigInstantiated.transform.position = new Vector3(fireballInHand.transform.position.x, fireballInHand.transform.position.y, fireballInHand.transform.position.z);
            }
            if (rightHandTransform.localPosition.x >= punchRange && returningRight == false || leftHandTransform.localPosition.x >= punchRange && returningLeft == false)
            {
                fireballInHand.SetActive(false);
                bigInstantiated.transform.position = GrabPosition.position;
                bigInstantiated.GetComponent<Rigidbody>().AddForce((transform.right) * (fireballSpeed), ForceMode.Impulse);
                bigInstantiated.GetComponentInChildren<SphereCollider>().enabled = true;
            }
            if (rightHandTransform.localPosition.x >= punchRange)
            {

                returningLeft = true;
                returningRight = true;
                punchedRight = false;
                punchedLeft = false;
                returnSpeed = 5f;
            }
            if (returningLeft)
            {

                animatorUpdated.SetBool("Grabbing", false);
                punchedLeft = false;
                leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector3(0, 0, 0), returnSpeed * Time.deltaTime);

                if (leftHandTransform.localPosition.x <= 1f)
                {
                    //leftHandCollider.enabled = false;
                }
                if (leftHandTransform.localPosition.x <= 0f)
                {
                    punchedRight = false;
                    punchedLeft = false;
                    grabbing = false;
                }
            }
            if (returningRight)
            {

                animatorUpdated.SetBool("Grabbing", false);
                punchedRight = false;
                rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(0, 0, 0), returnSpeed * Time.deltaTime);

                if (rightHandTransform.localPosition.x <= 1f)
                {
                    //rightHandCollider.enabled = false;

                }
                if (rightHandTransform.localPosition.x <= 0f)
                {

                    punchedRight = false;
                    punchedLeft = false;
                    grabbing = false;
                }
            }
            return;
        }
        #endregion

        animatorUpdated.SetBool("Grabbing", false);
        if (releasedLeft && fireballInstantiatedLeft != null && spawnedLeft == false && fireballInstantiatedLeft.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            spawnedLeft = true;
            GameObject newExplosion = Instantiate(explosionPrefab, fireballInstantiatedLeft.transform.position, Quaternion.identity);
            newExplosion.GetComponent<HandleCollider>().SetPlayer(this, leftHandTransform);
            ParticleSystem[] particles = fireballInstantiatedLeft.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem particle in particles)
            {
                particle.Stop();
            }
            fireballInstantiatedLeft.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        if (releasedRight && fireballInstantiated != null && spawnedRight == false && fireballInstantiated.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            spawnedRight = true;
            GameObject newExplosion = Instantiate(explosionPrefab, fireballInstantiated.transform.position, Quaternion.identity);
            newExplosion.GetComponent<HandleCollider>().SetPlayer(this, rightHandTransform);
            ParticleSystem[] particles = fireballInstantiated.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem particle in particles)
            {
                particle.Stop();
            }
            fireballInstantiated.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (punchedLeft && returningLeft == false)
        {
            spawnedLeft = false;
            punchedLeftTimer = 0;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector3(punchRange, -.4f, -.4f), punchSpeed * Time.deltaTime);
            if (leftHandTransform.localPosition.x >= punchRange)
            {

                fireballInstantiatedLeft = Instantiate(fireballPrefab, leftHandParent.transform.position, transform.rotation);
                fireballInHand.SetActive(false);

                fireballInstantiatedLeft.GetComponent<Rigidbody>().AddForce((transform.right) * (fireballSpeed), ForceMode.Impulse);
                returningLeft = true;
            }
        }
        if (returningLeft)
        {
            punchedLeft = false;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.transform.localPosition, new Vector3(0, 0, 0), returnSpeed * Time.deltaTime);


            if (leftHandTransform.localPosition.x <= 0f)
            {
                fireballInHand.SetActive(true);
                returningLeft = false;
            }
        }
        if (punchedRight && returningRight == false)
        {
            spawnedRight = false;
            punchedRightTimer = 0;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(punchRange, -.4f, .4f), punchSpeed * Time.deltaTime);
            if (rightHandTransform.localPosition.x >= punchRange)
            {
                fireballInstantiated = Instantiate(fireballPrefab, rightHandParent.transform.position, transform.rotation);
                fireballInHand.SetActive(false);

                returningRight = true;
                fireballInstantiated.GetComponent<Rigidbody>().AddForce((transform.right) * (fireballSpeed), ForceMode.Impulse); 
            }
        }
        if (returningRight)
        {
            punchedRight = false;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(0, 0, 0), returnSpeed * Time.deltaTime);


            if (rightHandTransform.localPosition.x <= 0f)
            {
                fireballInHand.SetActive(true);
                returningRight = false;
            }
        }

        if (punchedLeft || punchedRight)
        {
            moveSpeed = moveSpeedSetter - 8f;
        }

        if (!punchedLeft && !punchedRight)
        {
            moveSpeed = moveSpeedSetter;
        }
        if (shielding)
        {
            moveSpeed = 0;
        }
        if (shielding)
        {
            moveSpeed = 0;
        }

        if (state == State.Normal)
        {
            returnSpeed = 8f;
        }
        if (returningLeft && returningRight)
        {
            returnSpeed = 6f;
        }
        if (state == State.Dashing)
        {
            returnSpeed = 6f;
        }

    }
}
