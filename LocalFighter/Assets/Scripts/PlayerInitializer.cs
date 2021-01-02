using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInitializer : MonoBehaviour
{
    public GameObject thorPrefab;
    public GameObject fistPrefab;
    public GameObject firePrefab;
    public PlayerInputManager gameSceneManager;
    [SerializeField] private Transform[] playerSpawns;
    
    // Start is called before the first frame update
    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();

        for (int i = 0; i < playerConfigs.Length; i++)
        {

            if (playerConfigs[i].characterClass == 0)
            {
                Debug.Log(playerConfigs[i].PlayerIndex + "Player Index");
                //var player = Instantiate(fistPrefab, playerSpawns[i].position, playerSpawns[i].rotation);
                var player = PlayerInput.Instantiate(fistPrefab, playerConfigs[i].PlayerIndex, playerConfigs[i].currentControlScheme); //figure out how to get the device id
                //gameSceneManager.playerPrefab = fistPrefab;
                player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);
            }
            if (playerConfigs[i].characterClass == 1)
            {
                //var player = Instantiate(thorPrefab, playerSpawns[i].position, playerSpawns[i].rotation);
                var player = PlayerInput.Instantiate(thorPrefab, playerConfigs[i].PlayerIndex, playerConfigs[i].currentControlScheme); //figure out how to get the device id
                //gameSceneManager.playerPrefab = thorPrefab;
                player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);
            }
            if (playerConfigs[i].characterClass == 2)
            {
                //var player = Instantiate(thorPrefab, playerSpawns[i].position, playerSpawns[i].rotation);
                var player = PlayerInput.Instantiate(firePrefab, playerConfigs[i].PlayerIndex, playerConfigs[i].currentControlScheme); //figure out how to get the device id
                //gameSceneManager.playerPrefab = thorPrefab;
                player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
