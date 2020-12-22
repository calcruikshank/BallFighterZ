using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamSelect : MonoBehaviour
{
    public string name;
    public TMP_Text text;
    public int numOfRedPlayers = 0;
    public int numOfBluePlayers = 0;
    public bool selectedBlue = false;
    public bool selectedRed = false;
    public void OnClickSelectTeam(int teamSelected)
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.teamID = teamSelected;
            PlayerPrefs.SetInt("MyTeam", teamSelected);
            if (teamSelected == 1 && selectedBlue == false)
            {
                numOfBluePlayers++;
                text.text = (name + " " + numOfBluePlayers);
                if (selectedRed)
                {
                    numOfRedPlayers--;
                    text.text = (name + " " + numOfRedPlayers);
                }
                selectedBlue = true;
                
            }

            if (teamSelected == 0 && selectedRed == false)
            {
                numOfRedPlayers++;
                text.text = (name + " " + numOfRedPlayers);
                if (selectedBlue)
                {
                    numOfBluePlayers--;
                    text.text = (name + " " + numOfBluePlayers);
                }
                selectedRed = true;
                
            }
        }
        

    }
}
