using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InitializeLevel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform[] playerSpawns;
    GameObject percentText;
    [SerializeField] GameObject percentTextPrefab;
    public int gameMode = 0;
    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = PlayerInput.Instantiate(playerConfigs[i].PlayerPrefab, playerConfigs[i].PlayerIndex, playerConfigs[i].ControlScheme);
            player.GetComponent<TeamID>().SetColorOnMat(playerConfigs[i].PlayerColor);
            player.GetComponent<TeamID>().SetTeamID(playerConfigs[i].PlayerTeam);

            if (gameMode == 0) LoadClassic(player);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadClassic(PlayerInput player)
    {
        percentText = Instantiate(percentTextPrefab);
        percentText.gameObject.GetComponent<PercentTextBehaviour>().SetPlayer(player.gameObject.GetComponent<PlayerController>());
        percentText.transform.parent = FindObjectOfType<PercentageParent>().transform;
    }
}
