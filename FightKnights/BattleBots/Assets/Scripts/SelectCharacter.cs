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
        playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        PlayerInput pi = playerSent.gameObject.GetComponent<PlayerInput>();
        if (pi.currentControlScheme == "Gamepad")
        {
            playerInputManager.playerPrefab = PrefabToSpawn;
            var p1 = PlayerInput.Instantiate(PrefabToSpawn, pi.playerIndex, pi.currentControlScheme, pi.splitScreenIndex, Gamepad.current);
            p1.transform.position = playerSent.transform.position;
            p1.transform.right = playerSent.transform.right;
            PlayerInput p1I = p1.GetComponent<PlayerInput>();
        }
        if (pi.currentControlScheme == "Keyboard and Mouse")
        {
            playerInputManager.playerPrefab = PrefabToSpawn;
            var p1 = PlayerInput.Instantiate(PrefabToSpawn, pi.playerIndex, pi.currentControlScheme, pi.splitScreenIndex, Keyboard.current, Mouse.current);
            p1.transform.position = playerSent.transform.position;
            p1.transform.right = playerSent.transform.right;
            PlayerInput p1I = p1.GetComponent<PlayerInput>();
        }

        Destroy(playerSent.gameObject);
        playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
    }
}
