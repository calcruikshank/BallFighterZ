using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int PlayerIndex;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject readyPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button readyButton;
    [SerializeField] private Button backButton;

    private float ignoreInputTime = .3f;
    private bool inputEnabled;

    public void SetPlayerIndex(int pi)
    {
        PlayerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
        
    }


    public void SetClass(int characterClass)
    {
        if (!inputEnabled)
        {
            return;
        }

        PlayerConfigurationManager.Instance.SetPlayerClass(PlayerIndex, characterClass);
        Debug.Log(characterClass);
        readyPanel.SetActive(true);
        readyButton.Select();
        menuPanel.SetActive(false);
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled)
        {
            return;
        }
        PlayerConfigurationManager.Instance.ReadyPlayer(PlayerIndex);
        readyButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }

    public void BackFromReady()
    {
        if (!inputEnabled)
        {
            return;
        }
        readyPanel.SetActive(false);
        menuPanel.SetActive(true);
        menuPanel.GetComponentInChildren<Button>().Select();
    }

}
