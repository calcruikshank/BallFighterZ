using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGoal : MonoBehaviour
{

    public GameManager gameManager;
    public RedTeam redPlayer;
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
        redPlayer = hitInfo.GetComponent<RedTeam>();
        if (redPlayer != null)
        {
            gameManager.RedRespawn();
            redPlayer.currentPercentage = 0;
            redPlayer.damageText.text = redPlayer.currentPercentage.ToString() + "%";
        }
    }
}
