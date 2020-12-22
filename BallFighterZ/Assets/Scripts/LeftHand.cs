using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHand : MonoBehaviour
{
    public PlayerController opponent;
    public PlayerController player;
    public float downTicker;
    public Transform grabPosition;
    public LeftHand otherLeftHand;
    public RightHand otherRightHand;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (player.PV.IsMine) return;
        opponent = other.transform.parent.GetComponent<PlayerController>();
        
        //if opponent != null and hit opponents hand

        if (opponent != null) 
        {
            float damage = 6;
            Vector2 punchTowards = grabPosition.right.normalized;
            opponent.Knockback(damage, punchTowards);
            Debug.Log(damage + " damage beforeSending");
        }
    }

}
