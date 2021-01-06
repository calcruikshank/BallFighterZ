using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCollider : MonoBehaviour
{

    public PlayerController opponent;
    public PlayerController player;
    public float damage;
    public CapsuleCollider2D explosionCollider;
    // Start is called before the first frame update
    void Start()
    {
        explosionCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Explode");
        opponent = other.transform.parent.GetComponent<PlayerController>();
        if (opponent != null)
        {
            Debug.Log(other + "other explosion");
            opponent = other.transform.parent.GetComponent<PlayerController>();

            if (opponent != null && opponent != player)
            {
                if (player.isGrabbed)
                {

                    player.punchesToRelease++;
                    if (player.punchesToRelease >= 1)
                    {
                        player.EndGrab();
                        opponent.EndGrab();
                        opponent.isGrabbing = false;
                        damage = 4;
                        Vector2 punchTowards = player.grabPosition.right.normalized;
                        opponent.Knockback(damage, punchTowards);
                        explosionCollider.enabled = false;
                    }
                    return;

                }

                if (opponent.isBlockingLeft || opponent.isBlockingRight)
                {
                    if (opponent.isPowerShielding)
                    {
                        opponent.totalShieldRemaining += 20f / 255f;
                        opponent.PowerShield();
                        player.PowerShieldStun();
                        Debug.Log("Opponent is power shielding");
                        explosionCollider.enabled = false;
                        return;
                    }
                    opponent.totalShieldRemaining -= 10f / 255f;
                    explosionCollider.enabled = false;
                    return;
                }

                Debug.Log("Didnt grab");
                damage = 4;
                if (player.dashedTimer > 0f)
                {
                    damage = 20;
                    Debug.Log("took dash damage " + damage);
                }
                Vector2 knockTowards = new Vector2(opponent.transform.position.x - this.transform.position.x, opponent.transform.position.y - this.transform.position.y).normalized;
                //Vector2 handLocation = transform.position;
                opponent.rb.velocity = Vector3.zero;
                opponent.Knockback(damage, knockTowards);
                //opponent.Knockback(damage, handLocation);
                Debug.Log(damage + " damage beforeSending");
                explosionCollider.enabled = false;
            }
        }
    }

    public void SetPlayer(PlayerController playerset)
    {
        player = playerset;
    }
}
