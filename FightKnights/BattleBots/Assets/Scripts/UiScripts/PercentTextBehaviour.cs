using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PercentTextBehaviour : MonoBehaviour
{
    PlayerController player;
    [SerializeField]TextMeshProUGUI textObject;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log(textObject);
    }

    // Update is called once per frame
    void Update()
    {
        textObject.text = player.currentPercentage.ToString() + "%";
    }

    public void SetPlayer(PlayerController playerSent)
    {
        player = playerSent;
    }
}
