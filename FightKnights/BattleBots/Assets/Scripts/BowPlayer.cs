using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowPlayer : PlayerController
{
    float heldArrowTime;
    bool canShoot;
    [SerializeField] GameObject arrowPrefab, forcePushPrefab;
    GameObject arrowInstantiated;
    float arrowSpeed = 80f;
    protected override void HandleThrowingHands()
    {
        if (animatorUpdated != null)
        {
            animatorUpdated.SetBool("punchingRight", (punchedRight));
            animatorUpdated.SetBool("punchingLeft", (punchedLeft));

            animatorUpdated.SetBool("returningLeft", (returningLeft));
        }
        if (punchedLeft && returningLeft == false)
        {
            animatorUpdated.SetBool("Rolling", false);
            punchedLeftTimer = 0;
            //leftHandCollider.enabled = true;
            leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, new Vector3(punchRange, -.4f, -.4f), punchSpeed * Time.deltaTime);
            if (leftHandTransform.localPosition.x >= punchRange && returningLeft == false)
            {
                canShoot = true;
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

                animatorUpdated.SetBool("Grabbing", false);
            }
        }


        if (punchedRight)
        {
            if (!canShoot && punchedLeft) return;

            if (canShoot)
            {
                arrowInstantiated = Instantiate(arrowPrefab, GrabPosition.position, transform.rotation);
                arrowInstantiated.GetComponent<Rigidbody>().AddForce((transform.right) * (arrowSpeed), ForceMode.Impulse);
                arrowInstantiated.GetComponent<HandleCollider>().SetPlayer(this, rightHandTransform);
                canShoot = false;
            }

            
            animatorUpdated.SetBool("Rolling", false);
            punchedRightTimer = 0;
            //rightHandCollider.enabled = true;
            rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, new Vector3(punchRange, -.4f, .4f), punchSpeed * 2 * Time.deltaTime);
            if (rightHandTransform.localPosition.x >= punchRange)
            {
                if (!punchedLeft && !returningLeft && !punchedLeft)
                {
                    GameObject forcePushInst = Instantiate(forcePushPrefab, GrabPosition.position, transform.rotation);
                    forcePushInst.GetComponent<HandleCollider>().SetPlayer(this, rightHandTransform);
                }
                returningRight = true;
            }

            returningLeft = true;
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

                animatorUpdated.SetBool("Grabbing", false);
            }
        }
        if (grabbing) return;
        if (state == State.Dashing)
        {
            returnSpeed = 10f;
            moveSpeed = moveSpeedSetter + 15f;
            if (punchedLeft || punchedRight || returningLeft || returningRight)
            {
                moveSpeed = moveSpeedSetter + 2f;
            }
            return;
        }
        if (punchedLeft || punchedRight || returningLeft || returningRight)
        {
            moveSpeed = moveSpeedSetter - 12f;
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
            returnSpeed = 12f;
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


        if (returningLeft || punchedLeft) return;
        if (state == State.WaveDahsing && rb.velocity.magnitude > 10f) return;

        if (state == State.Knockback) return;
        if (state == State.Stunned) return;
        if (grabbing) return;
        if (state == State.Grabbed) return;

        if (punchedLeftTimer > 0)
        {
            if (lookDirection.magnitude != 0)
            {
                Vector3 lookTowards = new Vector3(lookDirection.x, 0, lookDirection.y);
                transform.right = lookTowards;
            }

            if (shielding) shielding = false;
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


        if (returningRight || punchedRight) return;
        if (state == State.WaveDahsing && rb.velocity.magnitude > 10f) return;

        if (state == State.Knockback) return;
        if (state == State.Stunned) return;
        if (grabbing) return;
        if (state == State.Grabbed) return;
        if (punchedRightTimer > 0)
        {
            if (lookDirection.magnitude != 0)
            {
                Vector3 lookTowards = new Vector3(lookDirection.x, 0, lookDirection.y);
                transform.right = lookTowards;
            }
            if (shielding) shielding = false;
            punchedRight = true;
            punchedRightTimer = 0;
        }
    }

    protected override void FaceLookDirection()
    {
        if (state == State.WaveDahsing) return;
        if (grabbing) return;
        if (punchedRight || returningRight && rightHandTransform.localPosition.x > punchRange - 1f) return;
        Vector3 lookTowards = new Vector3(lookDirection.x, 0, lookDirection.y);
        if (lookTowards.magnitude != 0f)
        {
            lastLookedPosition = lookTowards;
        }

        Look();
    }
}
