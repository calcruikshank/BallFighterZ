using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralGoal : MonoBehaviour
{
    public PlayerController player;
    public GameObject textBlueWonPrefab;
    public GameObject textRedWonPrefab;
    public GameObject restartText;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {

    }
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        player = other.transform.GetComponent<PlayerController>();
        if (player != null)
        {


            player.stocksLeft--;
            if (player.stocksLeft <= 0)
            {
                if (player.team == 0)
                {
                    textBlueWonPrefab.SetActive(true);
                    restartText.SetActive(true);
                    Destroy(player.gameObject);
                    Debug.Log("RedLost");
                    gameManager.gameIsOver = true;
                }
                if (player.team == 1)
                {
                    textRedWonPrefab.SetActive(true);
                    restartText.SetActive(true);
                    Destroy(player.gameObject);
                    Debug.Log("RedLost");
                    gameManager.gameIsOver = true;
                }

            }
            if (player.stocksLeft >= 0)
            {
                player.Respawn();
                //gameObject.transform.position = new Vector2(0, 0);
                Debug.Log("lost a stock");
            }

        }


    }
}
