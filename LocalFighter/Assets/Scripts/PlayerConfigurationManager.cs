using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;
    
    [SerializeField] private int MaxPlayers = 4;

    public static PlayerConfigurationManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }


    public void SetPlayerClass(int index, int character)
    {
        playerConfigs[index].characterClass = character;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].isReady = true;
        if (playerConfigs.Count <= MaxPlayers && playerConfigs.Count > 1 && playerConfigs.All(p => p.isReady == true))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player Joined" + pi.playerIndex);
        if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {

            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pi));
        }
    }
}

public class PlayerConfiguration
{
    public PlayerInput pcInput;
    public InputDevice deviceId;
    public PlayerConfiguration(PlayerInput pi)
    {
        pcInput = pi;
        PlayerIndex = pi.playerIndex;
        Input = pi;
        currentControlScheme = pi.currentControlScheme;
        deviceId = pi.devices[0];

    }
    public PlayerInput Input
    {
        get; set;
    }
    public int PlayerIndex
    {
        get; set;
    }
    public bool isReady
    {
        get; set;
    }
    public int characterClass
    {
        get; set;
    }
    
    public string currentControlScheme
    {
        get;
    }
    

    
}
