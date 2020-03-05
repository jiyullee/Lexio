using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyNetwork : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    private readonly string gameVersion = "1";
    public Lobby_PlayerList Lobby_PlayerList;
    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            print("Connecting to Server");
        }
    }


    public override void OnConnectedToMaster()
    {
        print("Connected to Master");
        PhotonNetwork.LocalPlayer.NickName = "지율";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedLobby()
    {
        print("Connected in lobby");
        Lobby_PlayerList.AddPlayer(PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnLeftLobby()
    {
        Lobby_PlayerList.RemovePlayer(PhotonNetwork.LocalPlayer.NickName);
    }

}
