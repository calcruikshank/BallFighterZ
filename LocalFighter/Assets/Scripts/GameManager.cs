using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int teamID = 0;
    public GameObject team0Prefab;
    public GameObject team1Prefab;
    public Transform text0;
    public Transform text1;
    public GameObject team0StockPrefab;
    public GameObject team1StockPrefab;
    public Transform text0Stock;
    public Transform text1Stock;
    public Transform text3;
    public Transform text4;
    public Transform text0Stock2;
    public Transform text1Stock2;
    public bool gameIsOver;
    int numOfTeamsLeft = 0;
    int numOfBluePlayers;
    int numOfRedPlayers;
    [SerializeField] GameObject textBlueWonPrefab, textRedWonPrefab, restartText;
    // Start is called before the first frame update
    void Start()
    {
        gameIsOver = false;
        /*PlayerController[] players = FindObjectsOfType<PlayerController>(); 
        foreach (PlayerController player in players)
        {
            
            
        }*/
    }

    public void SetTeam(PlayerController player)
    {
        player.team = teamID;
        SetText(teamID, player);
        teamID++;
        Debug.Log(player.team);
        if (player.team % 2 == 0)
        {
            numOfRedPlayers++;
        }
        if (player.team % 2 == 1)
        {
            numOfBluePlayers++;
        }

    }

    public void SetText(int spawnLocation, PlayerController player)
    {
        if (spawnLocation == 0)
        {
            GameObject textObject = Instantiate(team0Prefab, text0.position, Quaternion.identity);
            textObject.transform.SetParent(text0, false);
            player.percentageText = textObject.GetComponent<TMP_Text>();

            GameObject stockText = Instantiate(team0StockPrefab, text0Stock.position, Quaternion.identity);
            stockText.transform.SetParent(text0Stock, false);
            player.stocksLeftText = stockText.GetComponent<TMP_Text>();
        }
        if (spawnLocation == 1)
        {
            GameObject textObject = Instantiate(team1Prefab, text1.position, Quaternion.identity);
            textObject.transform.SetParent(text1, false);
            player.percentageText = textObject.GetComponent<TMP_Text>();

            GameObject stockText = Instantiate(team1StockPrefab, text1Stock.position, Quaternion.identity);
            stockText.transform.SetParent(text1Stock, false);
            player.stocksLeftText = stockText.GetComponent<TMP_Text>();
        }
        if (spawnLocation == 2)
        {
            GameObject textObject = Instantiate(team0Prefab, text3.position, Quaternion.identity);
            textObject.transform.SetParent(text3, false);
            player.percentageText = textObject.GetComponent<TMP_Text>();

            GameObject stockText = Instantiate(team0StockPrefab, text0Stock2.position, Quaternion.identity);
            stockText.transform.SetParent(text0Stock2, false);
            player.stocksLeftText = stockText.GetComponent<TMP_Text>();
        }
        if (spawnLocation == 3)
        {
            GameObject textObject = Instantiate(team1Prefab, text4.position, Quaternion.identity);
            textObject.transform.SetParent(text4, false);
            player.percentageText = textObject.GetComponent<TMP_Text>();

            GameObject stockText = Instantiate(team1StockPrefab, text1Stock2.position, Quaternion.identity);
            stockText.transform.SetParent(text1Stock2, false);
            player.stocksLeftText = stockText.GetComponent<TMP_Text>();
        }


    }


    public void RemoveBluePlayer(PlayerController player)
    {
        numOfBluePlayers--;
        Debug.Log("blue lost");
        if (numOfBluePlayers <= 0)
        {
            gameIsOver = true;
            textRedWonPrefab.SetActive(true);
            restartText.SetActive(true);
            //Destroy(player.gameObject);

            Debug.Log("blue lost inside if statement");
        }
    }

    public void RemoveRedPlayer(PlayerController player)
    {
        numOfRedPlayers--;
        if (numOfRedPlayers <= 0)
        {
            gameIsOver = true;
            textBlueWonPrefab.SetActive(true);
            restartText.SetActive(true);
            //Destroy(player.gameObject);
            Debug.Log("RedLost");
            //gameManager.gameIsOver = true;
        }
    }
}
