using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : MonoBehaviour
{
    float lifeTimer;
    float speed = 18f;
    PlayerController opponent;
    PlayerController player;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = 0f;
        this.gameObject.GetComponent<Rigidbody>().AddForce((player.transform.right) * (speed), ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer > 2f)
        {
            Destroy(this.gameObject);
        }
    }
    public void SetPlayer(PlayerController playerSent)
    {
        player = playerSent;
    }


    void OnTriggerEnter(Collider other)
    {
        opponent = other.transform.parent.GetComponent<PlayerController>();
        if (opponent != null && opponent != player)
        {
            Vector3 punchTowards = new Vector3(player.transform.right.normalized.x, 0, player.transform.right.normalized.z);
            float damage = 7;
            if (player.isDashing)
            {
                damage = 10f;
            }
            opponent.Knockback(damage, punchTowards, player);
            Physics.IgnoreCollision(this.transform.GetComponent<Collider>(), other);
            Destroy(this.gameObject);
        }
    }
}
