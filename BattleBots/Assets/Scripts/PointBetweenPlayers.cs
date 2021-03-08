using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBetweenPlayers : MonoBehaviour
{
    public PlayerController[] players;
    Vector3 pointToFollow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        players = FindObjectsOfType<PlayerController>();
        if (players.Length == 1)
        {
            pointToFollow = players[0].transform.position;
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(pointToFollow.x, pointToFollow.y + 25, pointToFollow.z - 20), 50 * Time.deltaTime);
            return;
        }
        foreach (PlayerController player in players)
        {
            pointToFollow += player.transform.position;
            pointToFollow = pointToFollow / players.Length;
        }


        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(pointToFollow.x, pointToFollow.y + 25, pointToFollow.z - 20), 50 * Time.deltaTime); 
    }
}
