using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueGoal : MonoBehaviour
{

    public BlueTeam bluePlayer;

    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        bluePlayer = hitInfo.GetComponent<BlueTeam>();
        if (bluePlayer != null)
        {
            gameManager.BlueRespawn();
            bluePlayer.currentPercentage = 0;
            bluePlayer.currentPercentage = 0;
            bluePlayer.damageText.text = bluePlayer.currentPercentage.ToString() + "%";
        }
    }
}
