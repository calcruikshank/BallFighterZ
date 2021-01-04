﻿using System.Collections;
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
    public bool gameIsOver;
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
        player.team = teamID % 2;
        SetText(teamID, player);
        teamID++;
        Debug.Log(player.team);
        
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

    }
}