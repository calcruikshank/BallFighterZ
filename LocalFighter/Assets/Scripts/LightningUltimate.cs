using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningUltimate : MonoBehaviour
{
    public PlayerController opponent;
    public PlayerController player;
    Collider2D thisCollider;
    float turnOffColliderAfterSeconds;
    // Start is called before the first frame update
    void Start()
    {
        thisCollider = this.transform.GetComponent<Collider2D>();
        thisCollider.enabled = true;
        turnOffColliderAfterSeconds = .1f;
    }

    // Update is called once per frame
    void Update()
    {
        turnOffColliderAfterSeconds -= Time.deltaTime;
        if (turnOffColliderAfterSeconds <= 0f)
        {
            thisCollider.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        opponent = other.transform.parent.GetComponent<PlayerController>();
        if (opponent != player && opponent != null)
        {
            /*if (opponent.isPowerShielding)
            {
                opponent.totalShieldRemaining += 20f / 255f;
                opponent.PowerShield();
                player.PowerShieldStun();
                Debug.Log("Opponent is power shielding");

                player.HitImpact(this.transform);
                return;
            }*/
            if (opponent.isPowerShielding)
            {
                opponent.totalShieldRemaining += 20f / 255f;
                opponent.PowerShield();
                opponent.rb.velocity = Vector3.zero;
                return;
            }
            Vector2 punchTowards = transform.right;
            opponent.rb.velocity = Vector3.zero;
            opponent.Knockback(30, punchTowards);

            Physics2D.IgnoreCollision(this.transform.GetComponent<Collider2D>(), other);
        }

    }


    public void SetPlayer(PlayerController playerWhoThrew)
    {
        player = playerWhoThrew;
    }
}
