﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_CreateRoom : MonoBehaviour
{
    public InputField roomName;
    public InputField passWord;
    public Dropdown playerCount;
    public Dropdown betting;

    private void OnEnable()
    {
        passWord.interactable = false;
    }

    public void TogglePassWord()
    {
        if(passWord.IsInteractable())
            passWord.interactable = false;
        else
            passWord.interactable = true;
    }
    public void OnClick_CreateRoom()
    {
        string bettingAmount = "";

        if (betting.value == 0)
            bettingAmount = "1백원";
        else if (betting.value == 1)
            bettingAmount = "1천원";
        else if (betting.value == 2)
            bettingAmount = "1만원";
        else if (betting.value == 3)
            bettingAmount = "10만원";
        else if (betting.value == 4)
            bettingAmount = "100만원";

        LobbyManager.Instance.OnClick_CreateRoom(roomName.text, passWord.text, playerCount.value + 2, bettingAmount);
        roomName.text = "";
        passWord.text = "";
        playerCount.navigation = Navigation.defaultNavigation;
        betting.navigation = Navigation.defaultNavigation;
        gameObject.SetActive(false);
    }

    public void OnClick_DisappearPanel()
    {
        roomName.text = "";
        passWord.text = "";
        gameObject.SetActive(false);
    }
}
