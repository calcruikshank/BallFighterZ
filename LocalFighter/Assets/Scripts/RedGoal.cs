using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class RedGoal : MonoBehaviour
{
    public PlayerController player;
    public GameObject textBlueWonPrefab;

    public GameObject restartText;
    public GameManager gameManager;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        player = other.transform.GetComponent<PlayerController>();
        if (player != null)
        {
            if (player.team == 0)
            {
                player.stocksLeft--;
                if (player.stocksLeft <= 0)
                {
                    textBlueWonPrefab.SetActive(true);
                    restartText.SetActive(true);
                    Destroy(player.gameObject);
                    Debug.Log("RedLost");
                    gameManager.gameIsOver = true;
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

}
