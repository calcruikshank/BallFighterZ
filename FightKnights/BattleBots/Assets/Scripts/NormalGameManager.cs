using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NormalGameManager : MonoBehaviour
{
    PercentageParent percentageParent;
    StockParent stockParent;

    // Start is called before the first frame update

    private void Awake()
    {
        percentageParent = FindObjectOfType<PercentageParent>();
        stockParent = FindObjectOfType<StockParent>();
    }
    void Start()
    {
        AddText();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddText()
    {
        PlayerInput[] players = FindObjectsOfType<PlayerInput>();
        foreach (PlayerInput player in players)
        {
            percentageParent.AddPercentageText(player.gameObject.GetComponent<PlayerController>());
            stockParent.AddPercentageText(player.gameObject.GetComponent<PlayerController>());
        }
    }
}
