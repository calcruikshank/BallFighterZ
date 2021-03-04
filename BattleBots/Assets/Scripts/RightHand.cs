using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHand : MonoBehaviour
{
    public PlayerController opponent;
    [SerializeField] Transform player;
    SphereCollider thisCollider;
    bool opponentTookDamage = false;

    // Start is called before the first frame update

    void Awake()
    {
        thisCollider = this.transform.GetComponent<SphereCollider>();
    }
    void Update()
    {
        if (transform.localPosition.x <= 0)
        {
            opponentTookDamage = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {

        opponent = other.transform.parent.GetComponent<PlayerController>();


        if (opponent != null)
        {
            if (!opponentTookDamage)
            {
                Debug.Log("Connected");
                Vector3 punchTowards = new Vector3(player.right.normalized.x, .1f, player.right.normalized.z);
                float damage = transform.localScale.x * 3f;
                opponent.Knockback(damage, punchTowards);
                Debug.Log(damage);
                opponentTookDamage = true;
            }
            
        }
        

    }
}
