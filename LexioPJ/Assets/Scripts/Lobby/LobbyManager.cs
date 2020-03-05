using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<LobbyManager>();

            return instance;
        }
    }

    private static LobbyManager instance;

    private string NickName;

    public InputField RoomName;
    public InputField RoomPlayerCount;
    public InputField InputPlayerNameText;

    [SerializeField]
    private RoomLayoutGroup _roomLayoutGroup;
    private RoomLayoutGroup RoomLayoutGroup
    {
        get { return _roomLayoutGroup; }
    }

    void Start()
    {
    }

    public void OnClick_CreateRoom()
    {
        int checkNum = 0;
        if (int.TryParse(RoomPlayerCount.text, out checkNum))
        {
            RoomOptions roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = (byte)int.Parse(RoomPlayerCount.text) };

            if(PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default))
            {
                print("Create Room success");
                
            }
        }
        
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Room");
    }

    public void OnClickJoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);                   
    }

    public void OnClick_ChangeName()
    {
        NickName = InputPlayerNameText.text;
        PhotonNetwork.LocalPlayer.NickName = NickName;
    }
}
