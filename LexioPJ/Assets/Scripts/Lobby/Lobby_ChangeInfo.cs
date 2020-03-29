using Photon.Pun;
using System;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_ChangeInfo : MonoBehaviour
{
    public void OnClick_DisappearPanel()
    {
        gameObject.SetActive(false);
    }

    public void OnClick_LogOut()
    {
        
        PlayFabClientAPI.ForgetAllCredentials();
        PlayerPrefs.SetInt("AutoLogin", 0);
        PhotonNetwork.LoadLevel("SignIn");

    }
}
