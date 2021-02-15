using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : PlayerController
{
    public override void Start()
    {
        brakeSpeed = 75f;
        state = State.Knockback;
    }

    

    public override void Knockback(float damage, Vector2 direction)
    {
        Debug.Log("Take knockback");
        isInKnockback = true;
        canAirShield = true;
        pressedAirShieldWhileInKnockback = false;
        canAirShieldTimer = 0f;
        //canAirShieldThreshold = .5f;
        StartCoroutine(cameraShake.Shake(.03f, .3f));
        shieldingLeft = false;
        shieldingRight = false;
        isBlockingLeft = false;
        isBlockingRight = false;
        shieldRightTimer = 0;
        shieldLeftTimer = 0;
        currentPercentage += damage;
        // Debug.Log(damage + " damage");
        //Vector2 direction = new Vector2(rb.position.x - handLocation.x, rb.position.y - handLocation.y); //distance between explosion position and rigidbody(bluePlayer)
        //direction = direction.normalized;
        float knockbackValue = 30f; //knockback that scales
        Debug.Log(knockbackValue);
        //canAirShieldThreshold = knockbackValue * .01f;
        rb.AddForce(direction * knockbackValue, ForceMode2D.Impulse);
        isGrabbed = false;
        //Debug.Log(currentPercentage + "current percentage");
        state = State.Knockback;
    }


    public override void HandleKnockback()
    {
        
        
        if (rb.velocity.magnitude <= 5)
        {
            isInKnockback = false;
            rb.velocity = new Vector2(0, 0);
            
        }
        if (rb.velocity.magnitude > 0)
        {

            oppositeForce = -rb.velocity;
            rb.AddForce(oppositeForce * Time.deltaTime * brakeSpeed);
            rb.AddForce(movement * .05f); //DI
        }
    }
}
