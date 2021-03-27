using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerGameManager : MonoBehaviour
{
    PlayerTeams playerTeams;
    // Start is called before the first frame update
    void Start()
    {
        playerTeams = FindObjectOfType<PlayerTeams>();
        if (!playerTeams.teamsIsOn)
        {
            playerTeams.ToggleTeamsOn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
