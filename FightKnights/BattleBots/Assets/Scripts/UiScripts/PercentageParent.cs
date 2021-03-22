using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PercentageParent : MonoBehaviour
{
    PlayerInputManager playerInputManager;
    public PlayerInput[] players;
    public List<PlayerController> playerList = new List<PlayerController>();
    [SerializeField] GameObject playerPercentText;
    GameObject percentText;
    // Start is called before the first frame update
    void Start()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
        players = FindObjectsOfType<PlayerInput>();
        foreach (PlayerInput playerInput in players)
        {
            PlayerController player = playerInput.GetComponent<PlayerController>();
            if (player != null && !playerList.Contains(player))
            {
                playerList.Add(player);
                AddPercentageText(player);
            }


        }
    }

    // Update is called once per frame
    void Update()
    {
        

        
    }



    void AddPercentageText(PlayerController player)
    {
        percentText = Instantiate(playerPercentText);
        percentText.transform.parent = this.transform;
        percentText.GetComponent<PercentTextBehaviour>().SetPlayer(player);
    }

    public void RemovePercentageText()
    {
        Destroy(percentText);
    }

    
}
