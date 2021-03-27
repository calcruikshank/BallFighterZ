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

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            textObject.text = player.currentPercentage.ToString() + "%";
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetPlayer(PlayerController playerSent)
    {
        player = playerSent;
    }
}
