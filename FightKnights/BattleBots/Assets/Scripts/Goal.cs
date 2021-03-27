using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public PlayerController player;
    public SoccerBall soccerBall;
    [SerializeField] int goalColor = -1; //if goal color is -1 its a neutral goal
    SoccerGameManager soccerGameManager;

    private void Start()
    {
        soccerGameManager = FindObjectOfType<SoccerGameManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        soccerBall = other.transform.parent.GetComponent<SoccerBall>();
        if (soccerBall != null)
        {
            if (goalColor == 0 && soccerGameManager != null)
            {
                soccerGameManager.AddScoreToBlue();

               
            }
            if (goalColor == 1 && soccerGameManager != null)
            {
                soccerGameManager.AddScoreToRed();


            }
            soccerBall.LoseStock();
            return;
        }

        player = other.transform.parent.GetComponent<PlayerController>();
        if (player != null)
        {
            player.LoseStock();
        }
        
        
    }
}
