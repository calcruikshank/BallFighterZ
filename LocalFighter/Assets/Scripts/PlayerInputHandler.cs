using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    public int index;
    public PlayerConfiguration playerConfig;
    
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (index == 0)
        {
            gameObject.transform.position = new Vector2(-10, 0);
        }
        if (index == 1)
        {
            gameObject.transform.position = new Vector2(10, 0);
        }
    }

    public int GetPlayerIndex()
    {
        return index;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializePlayer(PlayerConfiguration pc)
    {
        index = pc.PlayerIndex;
        
        //playerInput.playerIndex = pc.PlayerIndex;
        
    }
    

}
