using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHand : MonoBehaviour
{
    public PlayerController player;
    public PlayerController opponent;
    [SerializeField]GameObject explosionPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector2((transform.localPosition.x / 2) + 1, (transform.localPosition.x / 2) + 1); //sets the local scale equal to the local position + 1. the further punched the larger the scale. if local position is 0 then scale is one
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        opponent = other.transform.parent.GetComponent<PlayerController>();
        if (opponent != null)
        {
            if (opponent.isBlockingLeft || opponent.isBlockingRight)
            {
                if (opponent.isPowerShielding)
                {
                    Instantiate(explosionPrefab, transform.position, transform.rotation);
                    opponent.totalShieldRemaining += 20f / 255f;
                    opponent.PowerShield();
                    player.PowerShieldStun();
                    Debug.Log("Opponent is power shielding");
                    return;
                }
            }

            if (!opponent.isInKnockback)
            {
                player.RemoveFromComboCounter();
            }
            player.Grab(opponent);
            opponent.FireGrabbed(player.grabPosition);
            return;
        }
    }
}
