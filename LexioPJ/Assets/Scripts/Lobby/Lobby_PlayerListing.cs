using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_PlayerListing : MonoBehaviour
{
    public Text playerNameText;
    
    public void SetPlayerName(string s)
    {
        playerNameText.text = s;
    }
}
