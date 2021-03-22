using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectCharacter : PlayerController
{
    [SerializeField] GameObject PrefabToSpawn;
    [SerializeField] PlayerInputManager playerInputManager;

    public void Start()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }
    public override void Knockback(float damage, Vector3 direction, PlayerController playerSent)
    {
        //playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        PlayerInput pi = playerSent.gameObject.GetComponent<PlayerInput>();
        /*if (pi.currentControlScheme == "Gamepad")
        {
            var p1 = PlayerInput.Instantiate(PrefabToSpawn, pi.playerIndex, pi.currentControlScheme, pi.splitScreenIndex);
            p1.transform.position = playerSent.transform.position;
            p1.transform.right = playerSent.transform.right;
            PlayerInput p1I = p1.GetComponent<PlayerInput>();
        }*/


        playerInputManager.playerPrefab = PrefabToSpawn;
        Destroy(playerSent.gameObject);

        
        //playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
        
    }

    protected override void Look()
    {

    }
}
