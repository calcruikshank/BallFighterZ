using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLeftHand : MonoBehaviour
{
    public PlayerInputMouse redPlayer; //keep
    public bool isBlocking;

    public CircleCollider2D handCollider;
    float damage = 5f;
    public PlayerInputMouse playerScript;
    public Transform grabPosition;
    public BlueTeam bluePlayer;
    public bool didThrow = false;
    public bool isGrabbing = false;

    public float downTicker;
    public bool hitonefive = false;

    public bool hitHand = false;

    public bool isBlockingOnlyWithLeft;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isBlocking = redPlayer.isBlocking; //keep

        if (playerScript.punchedLeft)
        {
            handCollider.isTrigger = true;
        }
        if (playerScript.punchedLeft == false && isGrabbing == false || playerScript.isBlocking && isGrabbing == false)
        {
            handCollider.isTrigger = false;
        }
        if (transform.localPosition.x > 1.4f - .1f && playerScript.punchedLeft && hitonefive == false)
        {
            transform.localScale = new Vector2(1.5f, 1.5f);
            downTicker = 1.5f;
            hitonefive = true;
        }

        if (transform.localScale.x > 1f)
        {

            downTicker -= (3 * Time.deltaTime);

            transform.localScale = new Vector2(downTicker, downTicker);
            //Debug.Log(transform.localScale);

        }
        if (transform.localScale.x <= 1)
        {
            transform.localScale = new Vector2(1, 1);
            if (!playerScript.punchedLeft)
            {
                hitonefive = false;
            }

        }

        if (isGrabbing)
        {
            handCollider.isTrigger = true;


        }
        if (isGrabbing && playerScript.punchedLeft == false && playerScript.punchedRight == false && didThrow == false)
        {

            Vector2 throwTowards = grabPosition.right;

            if (bluePlayer != null)
            {
                bluePlayer.Throw(throwTowards);
            }

            didThrow = true;
            isGrabbing = false;
        }

        if (!playerScript.isBlocking)
        {
            isBlockingOnlyWithLeft = false;
        }
        if (playerScript.isBlocking && playerScript.returningLeft)
        {
            isBlockingOnlyWithLeft = true;
        }
    }


    void OnTriggerEnter2D(Collider2D hitInfo)
    {

        HandLeft handLeft = hitInfo.GetComponent<HandLeft>();
        HandRight handRight = hitInfo.GetComponent<HandRight>();
        if (handLeft != null)
        {
            //Debug.Log(redHand);

            if (handLeft.isBlocking)
            {

                playerScript.punchedLeft = false;
                playerScript.returningLeft = true;
                hitHand = true;
                return;
            }

        }
        if (handRight != null)
        {
            //Debug.Log(redHand);

            if (handRight.isBlocking) //have to check if right hand is punching as well trust me
            {

                playerScript.punchedLeft = false;
                playerScript.returningLeft = true;
                hitHand = true;
                return;
            }

        }



        didThrow = false;
        Vector2 handLocation = transform.position;
        Vector2 punchTowards = grabPosition.right;
        bluePlayer = hitInfo.GetComponent<BlueTeam>();

        if (isBlockingOnlyWithLeft == true && playerScript.punchedLeft)
        {
            damage = 1f;
            if (bluePlayer != null)
            {
                bluePlayer.TakeDamage(damage);
            }
            else
            {
                isGrabbing = false;
            }
            
        }






        //Debug.Log(hitInfo);
        if (bluePlayer != null && hitHand == false)
        {
            //Debug.Log(hitInfo);
            if (playerScript.punchedRight && playerScript.punchedLeft)
            {
                bluePlayer.rb.velocity = new Vector2(0, 0);
                bluePlayer.Grab(grabPosition);
                isGrabbing = true;

                return;
            }
            if (playerScript.punchedLeft == true && playerScript.isBlocking == false)
            {
                bluePlayer.rb.velocity = new Vector2(0, 0);
                damage = 4 * (downTicker * downTicker);
                bluePlayer.TakeDamage(damage);
                bluePlayer.Knockback(damage, punchTowards);
            }
            isGrabbing = false;

        }

        hitHand = false;

    }



    public void KnockOutOfGrab()
    {
        Vector2 throwTowards = grabPosition.right;
        if (bluePlayer != null)
        {
            bluePlayer.Throw(throwTowards);
        }
        isGrabbing = false;
        didThrow = true;
    }
}
