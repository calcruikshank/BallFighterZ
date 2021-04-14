using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InitializeLevel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform[] playerSpawns;

    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = PlayerInput.Instantiate(playerConfigs[i].PlayerPrefab, playerConfigs[i].PlayerIndex, playerConfigs[i].ControlScheme, -1, playerConfigs[i].CurrentDevice);
            player.GetComponent<TeamID>().SetColorOnMat(playerConfigs[i].PlayerColor);
            player.GetComponent<TeamID>().SetTeamID(playerConfigs[i].PlayerTeam);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
