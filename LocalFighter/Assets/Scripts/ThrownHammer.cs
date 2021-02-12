using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownHammer : MonoBehaviour
{
    public PlayerController opponent;
    public PlayerController player;
    public int whichHammer;
    bool opponentTookDamage = false;
    public GameObject explosionPrefab;
    public Transform pointOfContact;
    public Transform pointOfThunder;
    public GameObject lightningEffect;
    public GameObject lightingEffectLive;
    Collider2D hammerCollider;
    // Start is called before the first frame update
    void Start()
    {
        lightingEffectLive = Instantiate(lightningEffect, pointOfThunder.position, transform.rotation);
        hammerCollider = this.gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lightingEffectLive != null)
        {
            lightingEffectLive.transform.position = pointOfThunder.position;
        }
        if (this.transform.GetComponent<Rigidbody2D>().velocity.magnitude <= 1f)
        {
            Destroy(lightingEffectLive);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other != other.transform.parent.GetComponent<ThrownHammer>())
        {




            opponent = other.transform.parent.GetComponent<PlayerController>();
            if (opponent != player && opponent != null)
            {

                if (player.isGrabbed)
                {

                    player.punchesToRelease++;
                    opponentTookDamage = true;
                    hammerCollider.enabled = false;
                    //Physics2D.IgnoreCollision(hammerCollider, other);
                    if (player.punchesToRelease >= 1)
                    {

                        //player.HitImpact(this.transform);
                        player.EndGrab();
                        opponent.EndGrab();
                        opponent.isGrabbing = false;
                        float grabbeddamage = 6;
                        Instantiate(explosionPrefab, pointOfContact.position, transform.rotation);
                        Vector2 releaseTowards = transform.right;
                        if (this.GetComponent<Rigidbody2D>().velocity.magnitude <= .2f)
                        {
                            releaseTowards = -releaseTowards;
                        }
                        //Vector2 handLocation = transform.position;
                        opponent.rb.velocity = Vector3.zero;
                        opponent.Knockback(grabbeddamage, releaseTowards);
                    }
                    if (whichHammer == 0)
                    {
                        //Debug.Log("returnRightHammer");
                        player.GetComponent<Hammer>().ReturnRightHammer();
                    }
                    if (whichHammer == 1)
                    {
                        //Debug.Log("returnRightHammer");
                        player.GetComponent<Hammer>().ReturnLeftHammer();
                    }
                    return;
                }

                if (opponent.isBlockingLeft || opponent.isBlockingRight)
                {
                    if (opponent.isPowerShielding)
                    {
                        Instantiate(explosionPrefab, pointOfContact.position, transform.rotation);
                        opponent.totalShieldRemaining += 20f / 255f;
                        opponent.PowerShield();
                        player.PowerShieldStun();
                        Debug.Log("Opponent is power shielding");
                        opponentTookDamage = true;
                        hammerCollider.enabled = false;
                        //Physics2D.IgnoreCollision(hammerCollider, other);
                        //this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                        if (whichHammer == 0)
                        {
                            //Debug.Log("returnRightHammer");
                            player.GetComponent<Hammer>().ReturnRightHammer();
                        }
                        if (whichHammer == 1)
                        {
                            //Debug.Log("returnRightHammer");
                            player.GetComponent<Hammer>().ReturnLeftHammer();
                        }

                        return;

                    }
                    Instantiate(explosionPrefab, pointOfContact.position, transform.rotation);
                    opponentTookDamage = true;
                    hammerCollider.enabled = false;
                    //Physics2D.IgnoreCollision(hammerCollider, other);
                    //this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    opponent.totalShieldRemaining -= 10f / 255f;
                    if (whichHammer == 0)
                    {
                        //Debug.Log("returnRightHammer");
                        player.GetComponent<Hammer>().ReturnRightHammer();
                    }
                    if (whichHammer == 1)
                    {
                        //Debug.Log("returnRightHammer");
                        player.GetComponent<Hammer>().ReturnLeftHammer();
                    }

                    return;
                }
                Instantiate(explosionPrefab, pointOfContact.position, transform.rotation);
                float damage = 6;
                /*if (this.GetComponent<Rigidbody2D>().velocity.magnitude > .2f)
                {
                    //player.HitImpact(this.transform);
                }*/
                Vector2 punchTowards = transform.right;
                if (this.GetComponent<Rigidbody2D>().velocity.magnitude <= .2f)
                {

                    punchTowards = -punchTowards;
                }
                if (opponent.isInKnockback)
                {
                    player.AddToComboCounter();
                }
                if (!opponent.isInKnockback)
                {
                    player.RemoveFromComboCounter();
                }
                //Vector2 handLocation = transform.position;
                opponent.rb.velocity = Vector3.zero;
                opponent.Knockback(damage, punchTowards);
                //opponent.Knockback(damage, handLocation);
                //Debug.Log(damage + " damage beforeSending");
                opponentTookDamage = true;
                //this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                hammerCollider.enabled = false;
               // Physics2D.IgnoreCollision(hammerCollider, other);
            }


            if (whichHammer == 0)
            {
                //Debug.Log("returnRightHammer");
                player.GetComponent<Hammer>().ReturnRightHammer();
            }
            if (whichHammer == 1)
            {
                //Debug.Log("returnRightHammer");
                player.GetComponent<Hammer>().ReturnLeftHammer();
            }

        }


    }



    public void SetPlayer(PlayerController playerWhoThrew, int hand)
    {
        player = playerWhoThrew;
        whichHammer = hand;
    }


}
