using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : MonoBehaviour
{
    public GameObject lightningPrefab, instantiatedLightning;
    public PlayerController player;
    public GameObject arrowGameObject;
    public Transform arrowTransform;
    public float lifeTimer, strikeTimer;
    public bool isStriking = false;
    public PlayerController opponent;
    public CapsuleCollider2D lightningCollider;
    public float colliderTimer;
    public float buggedTimer;
    // Start is called before the first frame update
    void Start()
    {
        arrowGameObject.SetActive(false);
        arrowTransform = this.arrowGameObject.transform;
        strikeTimer = 0f;
        lifeTimer = 0f;
        isStriking = false;
        colliderTimer = 0f;
        buggedTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
       
        if (isStriking)
        {
            Strike();
        }
        if (!isStriking)
        {
            lifeTimer += Time.deltaTime;
        }
        if (lifeTimer > 1f)
        {
            colliderTimer += Time.deltaTime;
            instantiatedLightning = Instantiate(lightningPrefab, transform.position, arrowTransform.rotation);
            instantiatedLightning.GetComponent<LightningCollider>().SetPlayer(this.player);

            lightningCollider = instantiatedLightning.GetComponent<CapsuleCollider2D>();

            lightningCollider.enabled = true;
            //instantiateexplosion
            this.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            player.returningLeft = false;
            player.punchedLeft = false;
            if (colliderTimer > .01f)
            {
                lightningCollider.enabled = false;
            }
            Destroy(this.gameObject);
        }
        if (player.mousePosition.magnitude != 0)
        {
            Vector2 direction = new Vector2(player.mousePosition.x - transform.position.x, player.mousePosition.y - transform.position.y);
            arrowTransform.right = direction;
        }
        if (player.joystickLook.magnitude != 0)
        {
            arrowTransform.rotation = player.transform.rotation;
        }
    }

    public void SetPlayer(PlayerController playerSent)
    {
        player = playerSent;
    }
    public void Strike()
    {
        if(opponent == null)
        {
            colliderTimer += Time.deltaTime;
            instantiatedLightning = Instantiate(lightningPrefab, transform.position, arrowTransform.rotation);
            instantiatedLightning.GetComponent<LightningCollider>().SetPlayer(this.player);

            lightningCollider = instantiatedLightning.GetComponent<CapsuleCollider2D>();

            lightningCollider.enabled = true;
            //instantiateexplosion
            this.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            player.returningLeft = false;
            player.punchedLeft = false;
            if (colliderTimer > .01f)
            {
                lightningCollider.enabled = false;
            }
            Destroy(this.gameObject);
        }
        buggedTimer += Time.deltaTime;
        isStriking = true;
        this.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        arrowGameObject.SetActive(true);
        strikeTimer += Time.deltaTime;
        if (strikeTimer > .1f && opponent != null)
        {
            ExplodeLightning();
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Explode");
        opponent = other.transform.parent.GetComponent<PlayerController>();
        if (opponent != null && opponent != player)
        {
            this.transform.position = opponent.transform.position;

            if (!opponent.isInKnockback)
            {
                player.RemoveFromComboCounter();
            }
            opponent.ShockGrabbed();
            isStriking = true;

        }
    }

    public void ExplodeLightning()
    {
        
            //why the hell is opponent ever null please explain!!! it explodeslightning only if opponent isnt null on line 88
            Debug.Log("Exploding!!!!!!!!!!");
            instantiatedLightning = Instantiate(lightningPrefab, transform.position, arrowTransform.rotation);
            //instantiateexplosion
            this.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            player.returningLeft = false;
            player.punchedLeft = false;
            opponent.rb.velocity = Vector3.zero;
            opponent.Throw(arrowTransform.right);

            Destroy(this.gameObject);
        
        
    }
}
