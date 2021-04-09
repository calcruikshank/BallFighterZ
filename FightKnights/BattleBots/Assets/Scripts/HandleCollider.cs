using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCollider : MonoBehaviour
{
    public PlayerController player;
    PlayerController opponent;
    PlayerController opponentHit;
    float greatestDamage = 0f;
    Vector3 punchTowards;
    bool setDirection = false;
    [SerializeField] bool destroyedOnImpact = false;
    // Start is called before the first frame update
    void Start()
    {
        setDirection = false;
    }



    public void SetPlayer(PlayerController player, Transform handSent)
    {
        
        this.player = player;
    }

    public void HandleCollision(float hitId, float damage, PlayerController sentOpponent)
    {
        if (greatestDamage < damage)
        {
            greatestDamage = damage;
        }
        opponent = sentOpponent;
        if (sentOpponent != player && opponent != opponentHit)
        {
            if (opponent.isParrying)
            {
                opponent.Parry();
                player.ParryStun();
                return;
            }
            
            if (opponent.shielding)
            {
                return;
            }
            if (setDirection == false)
            {
                punchTowards = new Vector3(this.transform.right.normalized.x, 0, this.transform.right.normalized.z);
            }
            if (player != null)
            {
                if (player.isDashing)
                {
                    damage = 20f;
                }
            }

            Debug.Log("Knockback");
            opponent.Knockback(greatestDamage, punchTowards, player);
            opponentHit = sentOpponent;
            if (destroyedOnImpact)
            {
                this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                this.gameObject.GetComponentInChildren<Collider>().enabled = false;
                
                ParticleSystem[] particles = this.gameObject.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem particle in particles)
                {
                    if (particle != null)
                    {
                        particle.Stop();
                    }
                }
            }
        }
        greatestDamage = 0f;
    }

    public void SetKnockbackDirection(Vector3 direction)
    {
        
        
        punchTowards = direction;
        setDirection = true;

    }
}
