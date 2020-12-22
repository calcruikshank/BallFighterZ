using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHand : MonoBehaviour
{

    public PlayerController player, opponent;
    public float downTicker;
    public Transform grabPosition;
    public LeftHand otherLeftHand;
    public RightHand otherRightHand;

    public void OnTriggerEnter2D(Collider2D other)
    {
        
        opponent = other.transform.parent.GetComponent<PlayerController>();
        otherLeftHand = other.transform.GetComponent<LeftHand>();
        otherRightHand = other.transform.GetComponent<RightHand>();
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
