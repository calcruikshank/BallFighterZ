using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerPlayer : PlayerController
{
    [SerializeField] GameObject HammerPrefab;
    [SerializeField] GameObject HammerInHand;
    [SerializeField] GameObject LightningBall;
    GameObject ThrownHammer;
    GameObject lightningBallInstantiated;
    Rigidbody ThrownHammerRB;
    float hammerSpeed = 90f;
    float returnRightHammerSpeed;
    public bool returnHammer = false;
    Vector3 oppositeHammerForce;
    
    protected override void HandleThrowingHands()
    {
        if (animatorUpdated != null && ThrownHammer == null)
        {
            animatorUpdated.SetBool("punchingRight", (punchedRight));
            //animatorUpdated.SetBool("punchingLeft", (punchedLeft));

        }
        else
        {
            animatorUpdated.SetBool("punchingRight", (false));
        }
        animatorUpdated.SetBool("punchingLeft", (punchedLeft));
        animatorUpdated.SetBool("returningLeft", (returningLeft));

        if (punchedLeft && returningLeft == false)
        {

            punchedLeftTimer = 0;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector3(punchRange, -.4f, -.4f), punchSpeed * Time.deltaTime);
            if (leftHandTransform.localPosition.x >= punchRange)
            {
                lightningBallInstantiated = Instantiate(LightningBall, leftHandTransform.transform.position, transform.rotation);
                lightningBallInstantiated.GetComponent<LightningBall>().SetPlayer(this);
                returningLeft = true;
            }
        }
        if (returningLeft)
        {
            punchedLeft = false;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector3(0, 0, 0), returnSpeed * Time.deltaTime);

            
            if (leftHandTransform.localPosition.x <= 0f)
            {
                returningLeft = false;
            }
        }



        if (punchedRight && returningRight == false && ThrownHammer == null)
        {
            
            punchedRightTimer = 0;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(punchRangeRight, -.4f, .4f), punchSpeed * Time.deltaTime);
            if (rightHandTransform.localPosition.x >= punchRangeRight)
            {
                ThrownHammer = Instantiate(HammerPrefab, GrabPosition.position, transform.rotation);
                
                ThrownHammer.GetComponent<ThrownHammer>().SetPlayer(this);
                ThrownHammerRB = ThrownHammer.GetComponent<Rigidbody>();

                ThrownHammerRB.AddForce((transform.right) * (hammerSpeed), ForceMode.Impulse);
                HammerInHand.SetActive(false);
                returningRight = true;
            }
        }
        if (returningRight)
        {

            punchedRight = false;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(0, 0, 0), returnSpeed * Time.deltaTime);

            if (rightHandTransform.localPosition.x <= 0f)
            {
                
                returningRight = false;
            }
        }

        if (ThrownHammer == null)
        {

            returnHammer = false;
        }
        if (returnHammer && ThrownHammer != null)
        {
            if (oppositeHammerForce == Vector3.zero)
            {
                oppositeHammerForce = -ThrownHammerRB.velocity;
            }
            if (ThrownHammerRB.velocity.magnitude > 10f)
            {
                //Debug.Log("-hammer velocity = " + oppositeHammerForce);
                ThrownHammerRB.AddForce(oppositeHammerForce * 5 * Time.deltaTime, ForceMode.Impulse);
            }
            if (ThrownHammerRB.velocity.magnitude <= 10f)
            {
                ThrownHammerRB.velocity = Vector3.zero;

                returnRightHammerSpeed += 300 * Time.deltaTime;
                ThrownHammerRB.transform.position = Vector3.MoveTowards(ThrownHammerRB.transform.position, HammerInHand.transform.position, returnRightHammerSpeed * Time.deltaTime);
            }
            if (ThrownHammerRB.transform.position == HammerInHand.transform.position)
            {
                HammerInHand.SetActive(true);
                Destroy(ThrownHammer);
                returnHammer = false;
            }
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
        if (returningLeft && returningRight)
        {
            returnSpeed = 6f;
        }
        if (state == State.Dashing)
        {
            returnSpeed = 6f;
        }
    }


    public override void EndPunchRight()
    {
        punchedRight = false;
        returningRight = true;
        
        if (ThrownHammer != null)
        {
            returnHammer = true;
            oppositeHammerForce = -ThrownHammerRB.velocity;
            ThrownHammerRB.velocity = Vector3.zero;
            returnRightHammerSpeed = 0;
        }
    }
    protected override void CheckForPunchRight()
    {
        
        if (releasedRight)
        {
            punchedRightTimer -= Time.deltaTime;
            if (ThrownHammer != null && !returnHammer)
            {
                returnHammer = true;
                oppositeHammerForce = -ThrownHammerRB.velocity;
                returnRightHammerSpeed = 0;
            }
        }
        if (pressedRight)
        {
            punchedRightTimer = inputBuffer;
            pressedRight = false;
        }

        if (punchedLeft) return;
        if (returnHammer) return;
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
            
            punchedRight = true;
            punchedRightTimer = 0;
        }
    }

    protected override void CheckForPunchLeft()
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
        if (lightningBallInstantiated != null) return;
        

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
}
