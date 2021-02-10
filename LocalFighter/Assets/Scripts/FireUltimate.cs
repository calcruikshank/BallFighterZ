using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireUltimate : MonoBehaviour
{
    PlayerController opponent;
    PlayerController player;
    [SerializeField] GameObject ExplosionOnImpactPrefab;
    

    public void SetPlayer(PlayerController sentPlayer)
    {
        player = sentPlayer;
    }
}
