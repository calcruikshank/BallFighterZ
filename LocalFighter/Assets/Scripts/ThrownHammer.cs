﻿using System.Collections;
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
    // Start is called before the first frame update
    void Start()
    {
        lightingEffectLive = Instantiate(lightningEffect, pointOfThunder.position, transform.rotation);
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

            if (opponentTookDamage == false)
            {



                opponent = other.transform.parent.GetComponent<PlayerController>();
                if (opponent != player && opponent != null)
                {

                    if (player.isGrabbed)
                    {

                        player.punchesToRelease++;
                        opponentTookDamage = true;
                        if (player.punchesToRelease >= 1)
                        {
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
                    Vector2 punchTowards = transform.right;
                    if (this.GetComponent<Rigidbody2D>().velocity.magnitude <= .2f)
                    {
                        punchTowards = -punchTowards;
                    }
                    //Vector2 handLocation = transform.position;
                    opponent.rb.velocity = Vector3.zero;
                    opponent.Knockback(damage, punchTowards);
                    //opponent.Knockback(damage, handLocation);
                    //Debug.Log(damage + " damage beforeSending");
                    opponentTookDamage = true;
                }
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