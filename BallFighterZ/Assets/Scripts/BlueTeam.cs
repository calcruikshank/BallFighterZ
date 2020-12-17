using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueTeam : MonoBehaviour
{
    public GameObject bluePlayer;
    public PlayerInputController bluePlayerScript;
    public Rigidbody2D rb;
    
    public float currentPercentage;
    public Text damageText;
    public float knockbackValue;

    // Start is called before the first frame update
    void Awake()
    {
        bluePlayer = transform.parent.gameObject;
        bluePlayerScript = bluePlayer.GetComponent<PlayerInputController>();
        rb = bluePlayerScript.rb;
        damageText = GameObject.FindGameObjectWithTag("BluePlayerText").GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Throw(Vector2 throwTowards)
    {

        rb.AddForce(throwTowards * 10f, ForceMode2D.Impulse);

        bluePlayerScript.ChangeStateToKnockback();
    }


    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject + "took damage " + damage);
        currentPercentage += damage;
        //Debug.Log(gameObject + "took damage " + currentPercentage + "%");
        damageText.text = currentPercentage.ToString() + "%";
    }

    public void Knockback(float damage, Vector2 direction)
    {
        knockbackValue = (14 * ((currentPercentage + damage) * (damage / 3)) / 100) + 7; //knockback that scales

        bluePlayerScript.ChangeStateToKnockback();
        //Vector2 knockDirection = new Vector2(rb.position.x - direction.x, rb.position.y - direction.y);
        rb.AddForce(direction * knockbackValue, ForceMode2D.Impulse);
        Debug.Log("knockback value " + knockbackValue);
    }

    public void Grab(Transform handThatGrabbed)
    {
        bluePlayerScript.ChangeStateToGrabbed(handThatGrabbed);

        //rb.transform.position = grabLocation;
    }
}
