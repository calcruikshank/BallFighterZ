using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoccerGameManager : MonoBehaviour
{
    PlayerTeams playerTeams;
    SoccerCanvasBehaviour soccerCanvasBehavior;
    [SerializeField] GameObject soccerCanvas;
    Canvas canvas;

    public int redScore, blueScore = 0;
    // Start is called before the first frame update
    PercentageParent percentageParent;

    // Start is called before the first frame update

    private void Awake()
    {
        percentageParent = FindObjectOfType<PercentageParent>();
    }
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        playerTeams = FindObjectOfType<PlayerTeams>();

        AddText();

        if (!playerTeams.teamsIsOn)
        {
            playerTeams.ToggleTeamsOn();
        }

        GameObject soccerCanvasSpawned = Instantiate(soccerCanvas, Vector3.zero, Quaternion.identity);

        soccerCanvasSpawned.transform.position = Vector3.zero;
        soccerCanvasSpawned.transform.parent = canvas.transform;
        soccerCanvasSpawned.transform.localPosition = Vector3.zero;
        soccerCanvasBehavior = soccerCanvasSpawned.GetComponent<SoccerCanvasBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScoreToRed()
    {
        Debug.Log("AddToRed");
        redScore++;
        soccerCanvasBehavior.UpdateText(redScore, blueScore);
    }
    public void AddScoreToBlue()
    {
        Debug.Log("AddToBlue");
        blueScore++;
        soccerCanvasBehavior.UpdateText(redScore, blueScore);
    }

    public void AddText()
    {
        PlayerInput[] players = FindObjectsOfType<PlayerInput>();
        foreach (PlayerInput player in players)
        {
            if (player.gameObject.GetComponent<TeamID>() != null)
            {
                percentageParent.AddPercentageText(player.gameObject.GetComponent<PlayerController>());
            }
        }
    }
}
