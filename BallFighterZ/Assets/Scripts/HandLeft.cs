using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLeft : MonoBehaviour
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
    public bool isBlockingOnlyWithRight;


    public bool startDownTicker = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isBlocking = bluePlayer.isBlocking; //keep

        if (playerScript.punchedLeft)
        {
            handCollider.isTrigger = true;
        }
        if (playerScript.punchedLeft == false && isGrabbing == false || playerScript.isBlocking && isGrabbing == false)
        {
            handCollider.isTrigger = false;
        }

        if (transform.localPosition.x > playerScript.punchRange - .4f && playerScript.punchedLeft && hitonefive == false)
        {
            transform.localScale = new Vector2(1.5f, 1.5f);
            downTicker = 1.5f;
            hitonefive = true;
        }


        if (transform.localScale.x >= 1.4f)
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
            if (!playerScript.punchedLeft)
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
        if (isGrabbing && playerScript.punchedRight == false && playerScript.punchedLeft == false && didThrow == false)
        {
           
            Vector2 throwTowards = grabPosition.right;

                lastRedPlayer.Throw(throwTowards);
          
            didThrow = true;
            isGrabbing = false;
        }
        if (!playerScript.isBlocking)
        {
            isBlockingOnlyWithRight = false;
        }
        if (playerScript.isBlocking && playerScript.returningLeft)
        {
            isBlockingOnlyWithRight = true;
        }

    }

    void OnTriggerEnter2D (Collider2D hitInfo)
    {

        
        MouseLeftHand redHand = hitInfo.GetComponent<MouseLeftHand>();
        MouseRightHand redHandRight = hitInfo.GetComponent<MouseRightHand>();
        if (redHand != null)
        {
            //Debug.Log(redHand);

            if (redHand.isBlocking && !playerScript.CheckIfGrabbed())
            {

                playerScript.punchedLeft = false;
                playerScript.returningLeft = true;
                hitHand = true;
                return;
            }

        }
        if (redHandRight != null)
        {
            //Debug.Log(redHand);

            if (redHandRight.isBlocking && !playerScript.CheckIfGrabbed()) //have to check if right hand is punching as well trust me
            {

                playerScript.punchedLeft = false;
                playerScript.returningLeft = true;
                hitHand = true;
                return;
            }

        }

        didThrow = false;
        Vector2 handLocation = transform.position;
        Vector2 punchTowards = grabPosition.right.normalized;
        redPlayer = hitInfo.GetComponent<RedTeam>();
        //Debug.Log(hitInfo);


        if (isBlockingOnlyWithRight == true && playerScript.punchedLeft)
        {
            damage = 1f;
            redPlayer.TakeDamage(damage);
        }
        


        if (redPlayer != null && hitHand == false)
        {
            if (playerScript.punchedLeft && playerScript.punchedRight)
            {
                
                    redPlayer.rb.velocity = new Vector2(0, 0);
                    redPlayer.Grab(grabPosition);
                    isGrabbing = true;

                    return;
               
                
            }
           
            if (playerScript.punchedLeft == true && playerScript.isBlocking == false)
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
