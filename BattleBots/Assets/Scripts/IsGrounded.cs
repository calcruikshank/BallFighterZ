using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    Floor floor;
    public bool isGrounded = false;
    // Start is called before the first frame update
    void Start()
    {
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Grounded");
        floor = other.transform.GetComponent<Floor>();
        if (floor != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        floor = other.GetComponent<Floor>();
        if (floor != null)
        {
            isGrounded = false;
        }
        
    }
}
