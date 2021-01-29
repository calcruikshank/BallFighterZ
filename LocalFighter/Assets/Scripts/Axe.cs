using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : PlayerController
{
    public Transform axeTransformParent;
    public bool swungRight = false;
    public float swingSpeed = 10f;


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
        if (gameManager != null)
        {
            gameManager.SetTeam((PlayerController)this);

            stocksLeft = 4;
            stocksLeftText.text = (stocksLeft.ToString());
        }
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

    }

    public override void HandleThrowingHands()
    {
        punchedRightTimer -= Time.deltaTime;
        punchedLeftTimer -= Time.deltaTime;
        if (punchedLeftTimer > 0) punchedLeft = true;
        if (punchedRightTimer > 0) punchedRight = true;
        if (punchedRight)
        {
            Debug.Log("punchedRight");
            axeTransformParent.position = Vector3.MoveTowards(axeTransformParent.position, grabPosition.position, 20f * Time.deltaTime);
            Debug.Log(-axeTransformParent.localPosition.x);
            axeTransformParent.rotation = Quaternion.RotateTowards(axeTransformParent.rotation, grabPosition.rotation, 1 / -axeTransformParent.localPosition.x * -axeTransformParent.localPosition.x);
            if (axeTransformParent.position.x >= grabPosition.position.x)
            {
                swungRight = true;
                punchedRight = false;
            }
        }

        if (swungRight)
        {
            axeTransformParent.position = Vector3.MoveTowards(axeTransformParent.position, leftHandTransform.position, 20f * Time.deltaTime);
            axeTransformParent.rotation = Quaternion.RotateTowards(axeTransformParent.rotation, leftHandTransform.rotation, 4);
        }
    }
    public override void HandleShielding()
    {

    }

    public override void OnPunchRight()
    {
        if (state == State.Grabbing) return;
        if (state == State.Stunned) return;
        if (state == State.Dashing) return;
        if (state == State.Knockback) return;
        //Debug.Log("punchedRight");
        punchedRightTimer = inputBuffer;
        //punchedRight = true;
        shieldingRight = false;
    }
}
