using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedTeam : MonoBehaviour
{
    public GameObject redPlayer;
    public PlayerInputMouse redPlayerScript;
    public Rigidbody2D rb;
    public float currentPercentage;
    public Text damageText;
    public float knockbackValue;
    // Start is called before the first frame update
    void Start()
    {
        currentPercentage = 0;
        redPlayer = transform.parent.gameObject;
        redPlayerScript = redPlayer.GetComponent<PlayerInputMouse>();
        rb = redPlayerScript.rb;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        //Debug.Log(gameObject + "took damage " + damage);
        currentPercentage += damage;
        Debug.Log(gameObject + "took damage " + currentPercentage + "%");
        damageText.text = currentPercentage.ToString() + "%";
    }

    public void Knockback(float damage, Vector2 direction)
    {
        knockbackValue = (14 * ((currentPercentage + damage) * (damage / 3)) / 100) + 7; //knockback that scales

        redPlayerScript.ChangeStateToKnockback();
        //Vector2 knockDirection = new Vector2(rb.position.x - direction.x, rb.position.y - direction.y);
        rb.AddForce(direction * knockbackValue, ForceMode2D.Impulse);
        Debug.Log("knockback value " + knockbackValue);
    }

    public void Grab(Transform handThatGrabbed)
    {
        redPlayerScript.ChangeStateToGrabbed(handThatGrabbed);
        
        //rb.transform.position = grabLocation;
    }

    public void Throw(Vector2 throwTowards)
    {
        rb.AddForce(throwTowards * 10f, ForceMode2D.Impulse);
        redPlayerScript.ChangeStateToKnockback();
        
    }
}
