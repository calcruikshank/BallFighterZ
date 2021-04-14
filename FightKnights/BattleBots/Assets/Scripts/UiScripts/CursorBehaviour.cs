using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorBehaviour : MonoBehaviour
{
    Vector2 inputMovement;
    Vector3 movement, lastMoveDir;
    float moveSpeed = .5f;
    Button button;
    [SerializeField] GameObject multiEventSystem;
    GameObject myEventSystem;
    bool isReadied = false;
    GameObject currentSelectedPrefab;

    private int PlayerIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        myEventSystem = Instantiate(multiEventSystem);
        PlayerIndex = this.gameObject.GetComponent<PlayerInput>().playerIndex;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = inputMovement.x;
        movement.y = inputMovement.y;
        if (movement.x != 0 || movement.z != 0)
        {
            lastMoveDir = movement;
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

    public void SetCharacterChoice(GameObject character)
    {
        PlayerConfigurationManager.Instance.SetPlayerPrefab(PlayerIndex, character);
    }
    public void SetColor(Material color)
    {
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
