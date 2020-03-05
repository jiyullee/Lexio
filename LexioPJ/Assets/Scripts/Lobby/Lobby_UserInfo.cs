using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_UserInfo : MonoBehaviour
{
    public Text userNameText;
    public Text winPercentText;
    public Text GameMoneyText;

    private string PlayerName;
    private static int win;
    private static int lose;
    private int Money;

    private void Start()
    {
        userNameText.text = PlayerName;
    }
}
