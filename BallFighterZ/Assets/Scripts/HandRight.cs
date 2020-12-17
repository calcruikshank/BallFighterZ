using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRight : MonoBehaviour
{

    public PlayerInputController bluePlayer; //keep
    public bool isBlocking;

    public CircleCollider2D handCollider;
    float damage = 5f;
    public PlayerInputController playerScript;
    public Transform grabPosition;
    public RedTeam redPlayer;
    public bool didThrow = false;
    bool isGrabbing = false;
    public RedTeam lastRedPlayer;
    public float downTicker;
    public bool hitonefive = false;

    public bool hitHand = false;

    public bool startDownTicker = false;

    public bool isBlockingOnlyWithLeft;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isBlocking = bluePlayer.isBlocking; //keep

        if (playerScript.punchedRight)
        {
            handCollider.isTrigger = true;
        }
        if (playerScript.punchedRight == false && isGrabbing == false || playerScript.isBlocking && isGrabbing == false)
        {
            handCollider.isTrigger = false;
        }
        if (transform.localPosition.x > playerScript.punchRange - .4f && playerScript.punchedRight && hitonefive == false)
        {
            transform.localScale = new Vector2(1.5f, 1.5f);
            downTicker = 1.5f;
            hitonefive = true;
        }
        
        if (transform.localScale.x >= 1.5f && transform.localPosition.x >= playerScript.punchRange)
        {
            startDownTicker = true;
            
           //Debug.Log(transform.localScale);

        }

        if (startDownTicker)
        {
            downTicker -= (3 * Time.deltaTime);

            transform.localScale = new Vector2(downTicker, downTicker);
        }

        if (transform.localScale.x <= 1)
        {
            transform.localScale = new Vector2(1, 1);
            if (!playerScript.punchedRight)
            {
                hitonefive = false;
            }
            startDownTicker = false;

        }
        if (redPlayer != null)
        {
            lastRedPlayer = redPlayer;
        }
        if (isGrabbing)
        {
            handCollider.isTrigger = true;
            redPlayer = lastRedPlayer;

        }
        if (isGrabbing && playerScript.punchedLeft == false && playerScript.punchedRight == false && didThrow == false)
        {

            Vector2 throwTowards = grabPosition.right;

            lastRedPlayer.Throw(throwTowards);
            
            didThrow = true;
            isGrabbing = false;
        }

        if (!playerScript.isBlocking)
        {
            isBlockingOnlyWithLeft = false;
        }
        if (playerScript.isBlocking && playerScript.returningRight)
        {
            isBlockingOnlyWithLeft = true;
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        MouseLeftHand redHand = hitInfo.GetComponent<MouseLeftHand>();
        MouseRightHand redHandRight = hitInfo.GetComponent<MouseRightHand>();
        if (redHand != null)
        {
            //Debug.Log(redHand);
            
            if (redHand.isBlocking)
            {

                playerScript.punchedRight = false;
                playerScript.returningRight = true;
                hitHand = true;
                return;
            }
            
        }
        if (redHandRight != null)
        {
            //Debug.Log(redHand);

            if (redHandRight.isBlocking) //have to check if right hand is punching as well trust me
            {

                playerScript.punchedRight = false;
                playerScript.returningRight = true;
                hitHand = true;
                return;
            }

        }



        didThrow = false;
        Vector2 handLocation = transform.position;
        Vector2 punchTowards = grabPosition.right;
        redPlayer = hitInfo.GetComponent<RedTeam>();

        if (isBlockingOnlyWithLeft == true && playerScript.punchedRight && redPlayer != null)
        {
            damage = 1f;
            redPlayer.TakeDamage(damage);
        }






        //Debug.Log(hitInfo);
        if (redPlayer != null && hitHand == false)
        {
            //Debug.Log(hitInfo);
            if (playerScript.punchedRight && playerScript.punchedLeft)
            {


                    redPlayer.rb.velocity = new Vector2(0, 0);
                    redPlayer.Grab(grabPosition);
                    isGrabbing = true;

                    return;
                
                
            }

            if (playerScript.punchedRight == true && playerScript.isBlocking == false)
            {
                redPlayer.rb.velocity = new Vector2(0, 0);
                damage = 6 * (downTicker);
                redPlayer.TakeDamage(damage);
                redPlayer.Knockback(damage, punchTowards);
            }
            
            
        }

        hitHand = false;
    
    }
}
