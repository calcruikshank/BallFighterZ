using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float destroyTimer = 0f;
    public float destroyThreshold = 1f;
    public GameObject explosionFireballPrefab;
    public PlayerController opponent;
    public PlayerController player;
    public Transform tip;
    public float scaleSize;
    public GameObject instantiatedExplosion;
    public int whichHand;
    // Start is called before the first frame update
    void Start()
    {
        destroyTimer = 0f;
        scaleSize = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //scaleSize -= Time.deltaTime;
        //this.transform.localScale = new Vector2(scaleSize, scaleSize);
        /*destroyTimer += Time.deltaTime;
        if (destroyTimer > destroyThreshold)
        {
            instantiatedExplosion = Instantiate(explosionFireballPrefab, tip.position, this.transform.rotation);
            instantiatedExplosion.GetComponent<ExplosionScript>().SetPlayer(this.player);
            Destroy(gameObject);
        }*/
        

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other != null && other != other.transform.parent.GetComponent<ThrownHammer>())
        {
            instantiatedExplosion = Instantiate(explosionFireballPrefab, tip.position, this.transform.rotation);
            instantiatedExplosion.GetComponent<ExplosionScript>().SetPlayer(this.player);
            Destroy(gameObject);
            opponent = other.transform.parent.GetComponent<PlayerController>();
           
        }*/
    }

    public void SetPlayer(PlayerController playerWhoThrew, int handSent)
    {
        player = playerWhoThrew;
        whichHand = handSent;
    }

    public void DestroyRightFireball()
    {
        if (whichHand == 0)
        {
            instantiatedExplosion = Instantiate(explosionFireballPrefab, tip.position, this.transform.rotation);
            instantiatedExplosion.GetComponent<ExplosionScript>().SetPlayer(this.player);
            Destroy(this.gameObject);
        }
    }
    public void DestroyLeftFireball()
    {
        if (whichHand == 1)
        {
            instantiatedExplosion = Instantiate(explosionFireballPrefab, tip.position, this.transform.rotation);
            instantiatedExplosion.GetComponent<ExplosionScript>().SetPlayer(this.player);
            Destroy(this.gameObject);
        }
    }
}
