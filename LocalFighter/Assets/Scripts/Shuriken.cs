using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    PlayerController opponent, thisPlayer;
    float destroyAfterSecondsTimer = 0f;
    float destroyThreshold = .25f;
    int damage = 5;
    Vector3 knockTowards;
    Shuriken otherShuriken;
    // Start is called before the first frame update
    void Start()
    {
        destroyAfterSecondsTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        destroyAfterSecondsTimer += Time.deltaTime;
        if (destroyAfterSecondsTimer > destroyThreshold)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        otherShuriken = other.transform.GetComponent<Shuriken>();

        opponent = other.transform.parent.GetComponent<PlayerController>();
        if (opponent != null && opponent != thisPlayer)
        {
            if (thisPlayer.isGrabbed)
            {

                thisPlayer.punchesToRelease++;
                //Physics2D.IgnoreCollision(hammerCollider, other);
                if (thisPlayer.punchesToRelease >= 1)
                {
                    thisPlayer.EndGrab();
                    opponent.EndGrab();
                    opponent.isGrabbing = false;
                    float grabbeddamage = 4;
                    Vector2 releaseTowards = transform.right;
                    
                    //Vector2 handLocation = transform.position;
                    opponent.rb.velocity = Vector3.zero;
                    opponent.Knockback(grabbeddamage, releaseTowards);
                }
                Destroy(this.gameObject);

                return;
            }

            if (opponent.isBlockingLeft || opponent.isBlockingRight)
            {
                if (opponent.isPowerShielding)
                {
                    opponent.totalShieldRemaining += 20f / 255f;
                    opponent.PowerShield();
                    thisPlayer.PowerShieldStun();

                    Destroy(this.gameObject);
                    return;

                }
                opponent.totalShieldRemaining -= 10f / 255f;
                Destroy(this.gameObject);
                return;
            }
            if (opponent != null && opponent != thisPlayer)
            {
                if (opponent.isInKnockback && thisPlayer.canCombo)
                {
                    thisPlayer.canCombo = false;
                    thisPlayer.AddToComboCounter();
                }
                if (!opponent.isInKnockback)
                {
                    thisPlayer.RemoveFromComboCounter();
                }
                opponent.rb.velocity = Vector3.zero;
                knockTowards = transform.right;
                opponent.Knockback(damage, knockTowards);
            }


        }
        Destroy(this.gameObject);

    }

    public void SetPlayer(PlayerController playerSent)
    {
        thisPlayer = playerSent;
    }


}
