using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPlayer : PlayerController
{
    [SerializeField] GameObject swordSlash, swordThrustParticle, swordThrustSword, swordSlashSword;
    [SerializeField] Transform swordCrit, thrustPosition;

    protected override void HandleThrowingHands()
    {
        if (leftHandTransform.localPosition.x <= 0f)
        {

            swordThrustSword.SetActive(false);
            swordSlashSword.SetActive(true);
        }
        if (animatorUpdated != null)
        {
            animatorUpdated.SetBool("punchingRight", (punchedRight));
            animatorUpdated.SetBool("punchingLeft", (punchedLeft));

        }
        if (punchedLeft && returningLeft == false)
        {
            swordThrustSword.SetActive(true);
            swordSlashSword.SetActive(false);
            animatorUpdated.SetBool("Rolling", false);
            punchedLeftTimer = 0;
            //leftHandCollider.enabled = true;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector3(punchRange, -.4f, -.4f), (punchSpeed - 20) * Time.deltaTime);
            if (leftHandTransform.localPosition.x >= punchRange)
            {
                if (swordSlash != null)
                {
                    GameObject thrust = Instantiate(swordThrustParticle, thrustPosition.position, thrustPosition.rotation);
                    thrust.transform.right = new Vector3(thrustPosition.right.x, 0f, thrustPosition.right.z);
                    HandleCollider handleCollider = thrust.GetComponent<HandleCollider>();
                    handleCollider.SetPlayer(this, leftHandParent);
                }
                returningLeft = true;
            }
        }
        if (returningLeft)
        {
            punchedLeft = false;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector3(0, 0, 0), returnSpeed * Time.deltaTime);

            if (leftHandTransform.localPosition.x <= 1f)
            {
                //leftHandCollider.enabled = false;
            }
            if (leftHandTransform.localPosition.x <= 0f)
            {
                returningLeft = false;
            }
            if (leftHandTransform.localPosition.x <= 2f)
            {
                swordThrustSword.SetActive(false);
                swordSlashSword.SetActive(true);
            }
        }



        if (punchedRight && returningRight == false)
        {


            animatorUpdated.SetBool("Rolling", false);
            punchedRightTimer = 0;
            //rightHandCollider.enabled = true;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(punchRange, -.4f, .4f), punchSpeed * Time.deltaTime);
            if (rightHandTransform.localPosition.x >= punchRange)
            {
                if (swordSlash != null)
                {
                    GameObject slash = Instantiate(swordSlash, GrabPosition.position, Quaternion.identity);
                    slash.transform.right = transform.right;
                    HandleCollider handleCollider = slash.GetComponent<HandleCollider>();
                    handleCollider.SetPlayer(this, leftHandParent);
                }

                returningRight = true;
            }
        }
        if (returningRight)
        {
            punchedRight = false;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(0, 0, 0), returnSpeed * Time.deltaTime);

            if (rightHandTransform.localPosition.x <= 1f)
            {
                //rightHandCollider.enabled = false;
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
            returnSpeed = 8f;
        }
        if (returningLeft && returningRight)
        {

            returnSpeed = 4f;
        }
        if (state == State.Dashing)
        {

            returnSpeed = 4f;
        }
    }

    protected override void FaceLookDirection()
    {
        if (punchedLeft || punchedRight || returningLeft || rightHandTransform.localPosition.x > 2f && returningRight) if (state != State.Grabbing) return;
        if (state == State.WaveDahsing) return;
        if (state == State.Dashing) return;

        Vector3 lookTowards = new Vector3(lookDirection.x, 0, lookDirection.y);
        if (lookTowards.x != 0 || lookTowards.y != 0)
        {
            lastLookedPosition = lookTowards;
        }

        Look();
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

        if (returningLeft || punchedLeft) return;
        if (punchedRight || returningRight) return;
        if (state == State.WaveDahsing && rb.velocity.magnitude > 10f) return;
        if (shielding) return;
        if (state == State.Knockback) return;

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
        }
        if (returningLeft || punchedLeft) return;
        if (returningRight || punchedRight) return;
        if (state == State.WaveDahsing && rb.velocity.magnitude > 10f) return;
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


}
