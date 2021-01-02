using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BlueGoal : MonoBehaviour
{
    public PlayerController player;
    public GameObject textRedWonPrefab;
    public GameManager gameManager;
    public GameObject restartText;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        player = other.transform.GetComponent<PlayerController>();
        if (player != null)
        {
            if (player.team == 1)
            {
                player.stocksLeft--;
                if (player.stocksLeft <= 0)
                {

                    textRedWonPrefab.SetActive(true);
                    restartText.SetActive(true);
                    Destroy(player.gameObject);
                    Debug.Log("BlueLost");
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
