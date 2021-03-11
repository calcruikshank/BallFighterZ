using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLaunch : MonoBehaviour
{
    PlayerController player;
    PlayerController opponent;
    int hitID = 0;
    int damage = 11;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.GetComponent<HandleCollider>().SetKnockbackDirection(this.transform.right);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -20f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        opponent = other.transform.parent.GetComponent<PlayerController>();
        if (opponent != null && other != player)
        {

            this.transform.GetComponent<HandleCollider>().HandleCollision(hitID, this.gameObject.GetComponent<Rigidbody>().velocity.magnitude / 3, opponent);
            Physics.IgnoreCollision(other, this.transform.GetComponent<Collider>());
            Destroy(this.gameObject);
        }
    }

    public void SetPlayer(PlayerController player)
    {
        this.player = player;
        Debug.Log(player);
        Physics.IgnoreCollision(this.transform.GetComponent<Collider>(), player.GetComponentInChildren<Collider>());
    }
}
