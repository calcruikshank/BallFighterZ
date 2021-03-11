using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScript : MonoBehaviour
{
    PlayerController opponent;
    [SerializeField] int hitID;
    [SerializeField] float damage;

    private void OnTriggerEnter(Collider other)
    {
        opponent = other.transform.parent.GetComponent<PlayerController>();
        if (opponent != null)
        {
            
            this.transform.parent.transform.parent.GetComponent<PunchExplosion>().HandleCollision(hitID, damage, opponent);
            Physics.IgnoreCollision(other, this.transform.GetComponent<Collider>());
        }
    }
}
