using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyNetwork : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    private readonly string gameVersion = "1";
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
    }
}
