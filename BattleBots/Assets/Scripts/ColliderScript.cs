﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScript : MonoBehaviour
{
    PlayerController opponent;
    [SerializeField] int hitID;
    [SerializeField] float damage;
    [SerializeField] float colliderThreshold = 10f;
    float collideTimer;

    private void Update()
    {
        collideTimer += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {

        opponent = other.transform.parent.GetComponent<PlayerController>();
        if (opponent != null && collideTimer <= colliderThreshold)
        {
            
            this.transform.parent.transform.parent.GetComponent<HandleCollider>().HandleCollision(hitID, damage, opponent);
            Physics.IgnoreCollision(other, this.transform.GetComponent<Collider>());
        }
    }
}
