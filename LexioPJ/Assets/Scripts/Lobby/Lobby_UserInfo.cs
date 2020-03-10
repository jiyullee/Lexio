using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_UserInfo : MonoBehaviour
{
    public Text userNameText;
    public Text winPercentText;
    public Text GameMoneyText;
   
    private static int win;
    private static int lose;
    private int Money;
    private string playerName;
    private void Start()
    {
        if (PlayerPrefs.HasKey("Name"))
        {
            playerName = PlayerPrefs.GetString("Name");
            userNameText.text = playerName;
            PhotonNetwork.LocalPlayer.NickName = playerName;
        }
    }

    public void ChangeInfo()
    {

    }
}
