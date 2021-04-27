using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public PlayerController player;
    public SoccerBall soccerBall;
    [SerializeField] int goalColor = -1; //if goal color is -1 its a neutral goal

    
    void OnTriggerEnter(Collider other)
    {
        soccerBall = other.transform.parent.GetComponent<SoccerBall>();
        if (GameConfigurationManager.Instance != null)
        {
            if (GameConfigurationManager.Instance.gameMode == 1 && soccerBall != null)
            {
                if (goalColor == 0 && soccerBall.canBeScored)
                {
                    SoccerScore.Instance.AddToBlue();

                }
                if (goalColor == 1 && soccerBall.canBeScored)
                {


                    SoccerScore.Instance.AddToRed();
                }
                soccerBall.LoseStock();
                return;
            }
        }
        

        player = other.transform.parent.GetComponent<PlayerController>();
        if (player != null)
        {
            player.LoseStock();
        }
        
        
    }
}
