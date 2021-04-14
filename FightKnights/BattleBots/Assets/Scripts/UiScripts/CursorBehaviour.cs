using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class CursorBehaviour : MonoBehaviour
{
    Vector2 inputMovement;
    Vector3 movement, lastMoveDir;
    float moveSpeed = .5f;
    Button button;
    [SerializeField] GameObject multiEventSystem, playerInfo;
    GameObject myEventSystem;
    bool isReadied = false;
    GameObject currentSelectedPrefab, playerInfoInstantiated;

    Transform playerInfoParent;
    [SerializeField] public Color[] colors = new Color[6];
    private int PlayerIndex;
    string thisControlScheme;
    // Start is called before the first frame update
    void Start()
    {
        myEventSystem = Instantiate(multiEventSystem);
        PlayerIndex = this.gameObject.GetComponent<PlayerInput>().playerIndex;
        SetColor(colors[PlayerIndex]);
        SetTeam(PlayerIndex);
        playerInfoParent = FindObjectOfType<PlayerInfoParent>().transform;
        playerInfoInstantiated = Instantiate(playerInfo, Vector3.zero, Quaternion.identity);
        
        playerInfoInstantiated.transform.parent = playerInfoParent;
        playerInfo.transform.localScale = new Vector3(.8f, .8f, .8f);
        playerInfo.transform.localPosition = Vector3.zero;
        thisControlScheme = this.gameObject.GetComponent<PlayerInput>().currentControlScheme;
        SetControlScheme(thisControlScheme);
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = inputMovement.x;
        movement.y = inputMovement.y;
        if (movement.x != 0 || movement.z != 0)
        {
            lastMoveDir = movement;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10f);
    }

    private void FixedUpdate()
    {
        if (!isReadied)
        {
            this.transform.Translate(movement * moveSpeed);
        }
    }
    

    void SetControlScheme(string controlScheme)
    {
        PlayerConfigurationManager.Instance.SetControlScheme(PlayerIndex, controlScheme);
        if (controlScheme == "Gamepad")
        {
            Debug.Log("gamepad ");
            SetDevice(Gamepad.current);
        }
        else
        {
            Debug.Log("keyboard ");
            SetDevice(Keyboard.current);
        }
    }
    void SetDevice(InputDevice currentDevice)
    {
        PlayerConfigurationManager.Instance.SetDevice(PlayerIndex, currentDevice);
    }

    private void OnTriggerEnter(Collider other)
    {
        button = other.gameObject.GetComponent<Button>();
        currentSelectedPrefab = other.gameObject.GetComponent<CharChoiceButton>().prefabToSpawn;
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(button.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        currentSelectedPrefab = null;
        button = other.gameObject.GetComponent<Button>();
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    public void SetPlayerIndex(int pi)
    {
        PlayerIndex = pi;
    }

    public void SetTeam(int teamID)
    {
        PlayerConfigurationManager.Instance.SetPlayerTeam(PlayerIndex, teamID);
        playerInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Player " + (PlayerIndex + 1);
        if (teamID == 0)
        {
            playerInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Blue Team";
        }
        if (teamID == 1)
        {
            playerInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Red Team";
        }
        if (teamID == 2)
        {
            playerInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Yellow Team";
        }
        if (teamID == 3)
        {
            playerInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Green Team";
        }
        if (teamID == 4)
        {
            playerInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "White Team";
        }
        if (teamID == 5)
        {
            playerInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Black Team";
        }

    }
    public void SetCharacterChoice(GameObject character)
    {
        PlayerConfigurationManager.Instance.SetPlayerPrefab(PlayerIndex, character);
    }
    public void SetColor(Color charColor)
    {

        this.gameObject.GetComponentInChildren<Image>().color = charColor;
        PlayerConfigurationManager.Instance.SetPlayerColor(PlayerIndex, charColor);
        TextMeshProUGUI[] texts = playerInfo.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI tex in texts)
        {
            tex.color = charColor;
        }
    }

    

    public void ReadyPlayer()
    {
        //call this when you press a over a button
        PlayerConfigurationManager.Instance.ReadyPlayer(PlayerIndex);
        isReadied = true;
    }
    public void UnReadyPlayer()
    {
        //call this when you press b
        PlayerConfigurationManager.Instance.UnReadyPlayer(PlayerIndex);
        isReadied = false;
    }



    void OnMove(InputValue value)
    {
        inputMovement = value.Get<Vector2>();
    }

    void OnAButtonDown()
    {
        if (currentSelectedPrefab != null)
        {
            SetCharacterChoice(currentSelectedPrefab);
            ReadyPlayer();
        }
    }

    void OnAltPunchRight()
    {
        UnReadyPlayer();
    }

}
