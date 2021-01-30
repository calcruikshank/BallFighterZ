using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaHand : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void Update()
    {
        transform.localScale = new Vector2((transform.localPosition.x / 2) + 1, (transform.localPosition.x / 2) + 1); //sets the local scale equal to the local position + 1. the further punched the larger the scale. if local position is 0 then scale is one
    }
}
