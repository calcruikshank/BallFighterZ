using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public PlayerController player;
    [SerializeField] int goalColor = 0; //if goal color is 0 its a neutral goal
    void OnTriggerEnter(Collider other)
    {
        player = other.transform.parent.GetComponent<PlayerController>();
        if (player != null)
        {
            player.LoseStock();
        }
    }
}
