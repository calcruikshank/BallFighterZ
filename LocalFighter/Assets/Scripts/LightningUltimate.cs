using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningUltimate : MonoBehaviour
{
    public PlayerController opponent;
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        opponent = other.transform.parent.GetComponent<PlayerController>();
        if (opponent != player && opponent != null)
        {
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
