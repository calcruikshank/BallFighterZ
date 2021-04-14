using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;
    private int maxPlayers = 6;
    [SerializeField] Transform canvasInScene;
    public static PlayerConfigurationManager Instance { get; private set; }
    public PlayerInputManager pim;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Singleton - Trying to create another instance of a singleton");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
            pim = this.gameObject.GetComponent<PlayerInputManager>();
        }
    }

    public void SetPlayerPrefab(int index, GameObject characterChoice)
    {
        Debug.Log(characterChoice);
        playerConfigs[index].PlayerPrefab = characterChoice;
    }


    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;

        if (playerConfigs.Count >= 2 && playerConfigs.All(p => p.IsReady == true))
        {
            pim.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
            Debug.Log("Ready" + playerConfigs.Count);
            SceneManager.LoadScene(1);
        }
    }


    public void UnReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = false;
        playerConfigs[index].PlayerPrefab = null;
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player joined " + pi.playerIndex);

        if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            pi.transform.SetParent(canvasInScene);
            pi.transform.localScale = Vector3.one;
            playerConfigs.Add(new PlayerConfiguration(pi));
        }
    }
}


public class PlayerConfiguration
{

    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    
    public GameObject PlayerPrefab { get; set; }

    public Material PlayerMaterial { get; set; }
}
