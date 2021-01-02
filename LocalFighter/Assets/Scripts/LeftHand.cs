using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHand : MonoBehaviour
{
    public PlayerController player;
    public PlayerController opponent;
    public float downTicker = 1.5f;
    bool startDownTicker = false;
    bool opponentTookDamage = false;
    public GameObject explosionPrefab;


    void OnTriggerEnter2D(Collider2D other)
    {
        
        opponent = other.transform.parent.GetComponent<PlayerController>();
        

        if (opponent != null)
        {
            if (player.isGrabbed)
            {
                if (opponentTookDamage == false)
                {
                    player.punchesToRelease++;
                    opponent.AddDamage(2);
                    opponentTookDamage = true;
                    if (player.punchesToRelease >= 1)
                    {
                        player.EndGrab();
                        opponent.EndGrab();
                        opponent.isGrabbing = false;
                        float damage = 6;
                        Vector2 punchTowards = player.grabPosition.right.normalized;
                        opponent.Knockback(damage, punchTowards);
                    }
                    return;
                }

            }
            if (downTicker > 1 && player.punchedRight)
            {
                player.Grab(opponent);
                opponent.Grabbed(player.grabPosition);
                return;
            }
            if (opponent.isBlockingLeft || opponent.isBlockingRight)
            {
                Instantiate(explosionPrefab, transform.position, transform.rotation);
                //check if perfect shield
                if (opponent.isPowerShielding)
                {
                    opponent.totalShieldRemaining += 20f / 255f;
                    opponent.PowerShield();
                    player.PowerShieldStun();
                    opponentTookDamage = true;
                    Debug.Log("Opponent is power shielding");
                    return;
                }
                opponentTookDamage = true;
                opponent.totalShieldRemaining -= 10f / 255f;
                return;
            }

            if (player.isGrabbing && player.readyToPummelLeft)
            {
                opponent.TakePummelDamage();
                player.readyToPummelLeft = false;
            }

            if (player.returningLeft && transform.localPosition.x >= 1.25F || player.punchedLeft)
            {
                if (opponentTookDamage == false)
                {
                    Instantiate(explosionPrefab, transform.position, transform.rotation);
                    Debug.Log("Didnt grab");
                    float damage = 6;
                    if (player.dashedTimer > 0f)
                    {
                        damage = 20;
                        Debug.Log("took dash damage " + damage);
                    }
                    Vector2 punchTowards = player.grabPosition.right.normalized;
                    //Vector2 handLocation = transform.position;
                    opponent.rb.velocity = Vector3.zero;
                    opponent.Knockback(damage, punchTowards);
                    //opponent.Knockback(damage, handLocation);
                    Debug.Log(damage + " damage beforeSending");
                    opponentTookDamage = true;
                }
            }
            
        }
        
    }

    void Update()
    {
        if (player.punchedLeft)
        {
            startDownTicker = false;
            downTicker = 1.5f;
            transform.localScale = new Vector2(downTicker, downTicker);
        }
        if (player.returningLeft && transform.localScale.x > 1 || player.isGrabbing && transform.localScale.x > 1)
        {
            startDownTicker = true;
            
        }

        if (startDownTicker)
        {
            downTicker -= (3 * Time.deltaTime);
            transform.localScale = new Vector2(downTicker, downTicker);
            if (downTicker <= 1)
            {
                startDownTicker = false;
            }
        }
        if (player.returningLeft == false && player.punchedLeft == false)
        {
            opponentTookDamage = false;
        }
    }
}
