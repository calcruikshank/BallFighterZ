using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject redPlayer;
    public GameObject bluePlayer;

    public int redPlayerStocks = 3;
    public int bluePlayerStocks = 3;

    public Transform redPlayerRespawn;
    public Transform bluePlayerRespawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BlueRespawn()
    {
        Transform bluePlayerTransform = GameObject.FindGameObjectWithTag("BluePlayer").transform;
        Rigidbody2D blueRB = GameObject.FindGameObjectWithTag("BluePlayer").GetComponent<Rigidbody2D>();
        bluePlayerStocks--;
        
        if (bluePlayerStocks <= 0)
        {
            RedTeamVictory();
        }
        
        Debug.Log(bluePlayerStocks);
       
        bluePlayerTransform.position = new Vector2(0, 0);
        blueRB.velocity = Vector3.zero;
    }

    public void RedRespawn()
    {
        Transform redPlayerTransform = GameObject.FindGameObjectWithTag("RedPlayer").transform;
        Rigidbody2D redRB = GameObject.FindGameObjectWithTag("RedPlayer").GetComponent<Rigidbody2D>();
        redPlayerStocks--;
        if (redPlayerStocks <= 0)
        {
            RedTeamVictory();
        }

        Debug.Log(redPlayerStocks);
        redPlayerTransform.position = new Vector2(0, 0);
        redRB.velocity = Vector3.zero;
    }

    public void RedTeamVictory()
    {
        Debug.Log("Red team won");
    }
}
