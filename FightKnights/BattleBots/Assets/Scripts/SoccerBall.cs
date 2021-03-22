using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBall : PlayerController
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected override void Update()
    {
        base.Update();
        if (state != State.Grabbed && state != State.Stunned)
        {
            state = State.Knockback;
        }
    }
    protected override void Look()
    {
        
    }

    protected override void HandleThrowingHands()
    {
    }
    public override void Knockback(float damage, Vector3 direction, PlayerController playerSent)
    {
        canAirDodgeTimer = 0f;

        currentPercentage += damage;
        brakeSpeed = 10f;
        // Debug.Log(damage + " damage");
        //Vector2 direction = new Vector2(rb.position.x - handLocation.x, rb.position.y - handLocation.y); //distance between explosion position and rigidbody(bluePlayer)
        //direction = direction.normalized;
        float knockbackValue = (20 * ((60 + damage) * (damage / 2)) / 150) + 14; //knockback that scales
        rb.velocity = new Vector3(direction.x * knockbackValue, 0f, direction.z * knockbackValue);

        HitImpact(direction);
        state = State.Knockback;
    }
    protected override void HandleKnockback()
    {

    }

    protected override void Respawn()
    {
        state = State.Normal;
        transform.position = Vector3.zero;
        currentPercentage = 0f;
    }
}
