using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHand : MonoBehaviour
{
    public PlayerController player;
    public PlayerController opponent;
    public float downTicker = 1.5f;
    bool startDownTicker;
    bool opponentTookDamage;
    public GameObject explosionPrefab;
    CircleCollider2D thisCollider;


    void Start()
    {
        thisCollider = this.transform.GetComponent<CircleCollider2D>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
        opponent = other.transform.parent.GetComponent<PlayerController>();
        
        

        if (opponent != null)
        {
            if (player.isUlting)
            {
                player.HitImpact(this.transform);

                opponent.AddDamage(5);
                opponent.TakeUltimate(player.grabPosition);
                player.ultConnect = true;
                if (player.ultPunchCounter >= 14)
                {
                    player.EndUlt();

                    Vector2 punchTowards = player.grabPosition.right.normalized;
                    opponent.Knockback(10, punchTowards);
                }
                return;
            }


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
                        thisCollider.enabled = false;
                    }
                    return;
                }
            }


            if (opponent.isPowerShielding)
            {
                opponent.totalShieldRemaining += 20f / 255f;
                opponent.PowerShield();
                thisCollider.enabled = false;
                player.PowerShieldStun();
                Debug.Log("Opponent is power shielding");

                player.HitImpact(this.transform);
                return;
            }
            if (downTicker > 1 && player.punchedLeft)
            {
                player.Grab(opponent);
                opponent.Grabbed(player.grabPosition);
                return;
            }

            if (opponent.isBlockingLeft || opponent.isBlockingRight)
            {
                Instantiate(explosionPrefab, transform.position, transform.rotation);
                //check if perfect shield
                

                player.HitImpact(this.transform);
                opponent.totalShieldRemaining -= 10f / 255f;
                return;
            }

            if (player.isGrabbing && player.readyToPummelRight)
            {
                opponent.TakePummelDamage();
                player.readyToPummelRight = false;
            }
            
            if (player.returningRight && transform.localPosition.x >= 1.5F || player.punchedRight)
            {
                if (opponentTookDamage == false)
                {

                    Instantiate(explosionPrefab, transform.position, transform.rotation);
                    Debug.Log("Didnt grab");
                    float damage = 4 * transform.localScale.x;
                    if (player.dashedTimer > 0f)
                    {
                        damage = 15;
                        Debug.Log("took dash damage " + damage);
                    }
                    Vector2 punchTowards = player.grabPosition.right.normalized;
                    //Vector2 handLocation = transform.position;
                    opponent.rb.velocity = Vector3.zero;
                    if (opponent.isInKnockback)
                    {
                        player.AddToComboCounter();
                    }
                    if (!opponent.isInKnockback)
                    {
                        player.RemoveFromComboCounter();
                    }
                    player.HitImpact(this.transform);
                    opponent.Knockback(damage, punchTowards);
                    thisCollider.enabled = false;
                    //opponent.Knockback(damage, handLocation);
                    Debug.Log(damage + " damage beforeSending");
                    opponentTookDamage = true;
                    
                }
                
            }
            thisCollider.enabled = false;
        }
        
    }


    void Update()
    {
        /*if (player.punchedRight)
        {
            startDownTicker = false;
            downTicker = 2f;
            transform.localScale = new Vector2(downTicker, downTicker);
            
        }
        if (player.returningRight && transform.localScale.x > 1 || player.isGrabbing && transform.localScale.x > 1)
        {
            startDownTicker = true;
        }

        if (startDownTicker)
        {
            downTicker -= (3f * Time.deltaTime);
            transform.localScale = new Vector2(downTicker, downTicker);
            if (downTicker <= 1)
            {
                startDownTicker = false;
            }
        }*/
        transform.localScale = new Vector2((transform.localPosition.x / 2) + 1, (transform.localPosition.x / 2) + 1);


        if (player.returningRight == false && player.punchedRight == false)
        {
            opponentTookDamage = false;
        }
    }
}
