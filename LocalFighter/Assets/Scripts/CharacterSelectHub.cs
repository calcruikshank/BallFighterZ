using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectHub : MonoBehaviour
{
    PlayerController playerInsideBounds;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        playerInsideBounds = other.transform.parent.GetComponent<PlayerController>();
        if (playerInsideBounds != null)
        {
            playerInsideBounds.canSelectCharacter = true;
        }
    }
    void OnTriggerExit()
    {
        if (playerInsideBounds != null)
        {
            playerInsideBounds.canSelectCharacter = false;
        }
    }
}
