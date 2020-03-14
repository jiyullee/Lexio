using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class Lobby_CheckPassword : MonoBehaviour
{
    private InputField inputPassword;
    private string roomName;
    private string password;
    
    private void Start()
    {
        inputPassword = GetComponentInChildren<InputField>();
    }

    public void SetProperties(string roomName)
    {
        this.roomName = roomName;
    }
    public void CheckPassword(string inputPassword)
    {
        ExitGames.Client.Photon.Hashtable expectedCustomProperties = new ExitGames.Client.Photon.Hashtable()
        {
            { "RoomName", roomName }, {"Password", inputPassword},
        };
        PhotonNetwork.JoinRandomRoom(expectedCustomProperties, 0);

    }

    public void OnClick_InputPassword()
    {
        CheckPassword(inputPassword.text);
    }

    public void Onclick_CancelPassword()
    {
        gameObject.SetActive(false);
    }
}
