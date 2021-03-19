using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleColliderShieldBreak : MonoBehaviour
{
    public PlayerController player;
    PlayerController opponent;
    PlayerController opponentHit;
    float greatestDamage = 0f;
    Vector3 punchTowards;
    // Start is called before the first frame update
    void Start()
    {

    }




    public void SetPlayer(PlayerController player, Transform handSent)
    {

        this.player = player;
    }

    public void HandleCollision(float hitId, float damage, PlayerController sentOpponent, float stunTime)
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
                opponent.Stunned(stunTime, damage);
                opponentHit = sentOpponent;
                return;
            }
            if (punchTowards == null || punchTowards == Vector3.zero)
            {
                punchTowards = new Vector3(player.transform.right.normalized.x, 0, player.transform.right.normalized.z);
            }

            if (player.isDashing)
            {
                damage = 20f;
            }
            opponent.Knockback(greatestDamage, punchTowards, player);
            opponentHit = sentOpponent;
        }
        greatestDamage = 0f;
    }

    public void SetKnockbackDirection(Vector3 direction)
    {


        punchTowards = direction;

    }
}
