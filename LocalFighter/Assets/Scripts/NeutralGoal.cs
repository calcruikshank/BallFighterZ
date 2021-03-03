using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralGoal : MonoBehaviour
{
    public PlayerController player;
    public GameObject textBlueWonPrefab;
    public GameObject textRedWonPrefab;
    public GameObject restartText;
    public GameManager gameManager;
    [SerializeField] ParticleSystem particle;

    ScreenShake cameraShake;
    // Start is called before the first frame update
    void Start()
    {

    }
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        cameraShake = FindObjectOfType<ScreenShake>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        player = other.transform.GetComponent<PlayerController>();
        if (player != null)
        {


            player.stocksLeft--;
            if (player.stocksLeft <= 0)
            {
                if (player.team % 2 == 0)
                {
                    gameManager.RemoveRedPlayer(player);
                    Destroy(player.animationTransformHandler.gameObject);
                    Destroy(player.gameObject);
                    //textBlueWonPrefab.SetActive(true);
                    //restartText.SetActive(true);
                    //Destroy(player.gameObject);
                    //Debug.Log("RedLost");
                    //gameManager.gameIsOver = true;
                }
                if (player.team % 2 == 1)
                {

                    gameManager.RemoveBluePlayer(player);
                    Destroy(player.animationTransformHandler.gameObject);
                    Destroy(player.gameObject);
                    //textRedWonPrefab.SetActive(true);
                    //restartText.SetActive(true);
                    //Destroy(player.gameObject);
                    //Debug.Log("RedLost");
                    //gameManager.gameIsOver = true;
                }

            }
            if (player.stocksLeft >= 0)
            {
                StartCoroutine(cameraShake.Shake(.05f, .5f));
                particle.Play();
                player.Respawn();
                //gameObject.transform.position = new Vector2(0, 0);
                Debug.Log("lost a stock");
            }

            AudioManager._Main.PlayGoal();
            AudioManager._Main.PlayAnnouncer(1);
        }


    }
}
