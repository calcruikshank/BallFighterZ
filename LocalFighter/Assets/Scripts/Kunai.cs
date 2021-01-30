using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    PlayerController opponent, thisPlayer;
    float destroyAfterSecondsTimer = 0f;
    float destroyThreshold = .5f;
    int damage = 8;
    Vector2 knockTowards;
    // Start is called before the first frame update
    void Start()
    {
        
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
            }
            if (opponent != null && opponent != thisPlayer)
            {
                opponent.rb.velocity = Vector3.zero;
                knockTowards = transform.right;
                opponent.NinjaThrow(knockTowards);
            }


        }
        Destroy(this.gameObject);

    }


    public void SetPlayer(NinjaCS sentPlayer)
    {
        thisPlayer = sentPlayer;
        sentPlayer = (NinjaCS)sentPlayer;
        Physics2D.IgnoreCollision(this.transform.GetComponent<CircleCollider2D>(), sentPlayer.hitBox);
    }
}
