using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler Instance
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
        }
    }


    PlayerInput playerInput;
    public GameObject fistPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCharacter(int characterClass, GameObject gameObjectToReplace)
    {
        playerInput = gameObjectToReplace.GetComponent<PlayerInput>();
        if (characterClass == 0)
        {
            var player = PlayerInput.Instantiate(fistPrefab, playerInput.playerIndex, playerInput.currentControlScheme, 0, playerInput.devices[playerInput.playerIndex]);
            Destroy(gameObjectToReplace);
        }
    }
}
