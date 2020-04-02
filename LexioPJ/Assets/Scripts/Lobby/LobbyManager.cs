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
    public GameObject passwordChecker;
    InputField inputPassword;
    public Lobby_ChatService Lobby_ChatService;
    [SerializeField]
    private RoomLayoutGroup _roomLayoutGroup;
    private RoomLayoutGroup RoomLayoutGroup
    {
        get { return _roomLayoutGroup; }
    }
    string s;
    public LevelLoader LevelLoader;
    
    private void Awake()
    {
        Screen.SetResolution(1280, 720, false);
    }
    private void Start()
    {
        inputPassword = passwordChecker.GetComponentInChildren<InputField>();
    }

    
    public void OnClick_CreateRoom(string roomName,  string password, int maxCount, string betting)
    {
        RoomOptions roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = (byte)maxCount };
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();

        roomOptions.CustomRoomProperties.Add("RoomName", roomName);
        roomOptions.CustomRoomProperties.Add("Password", password);
        roomOptions.CustomRoomProperties.Add("MasterName", PhotonNetwork.LocalPlayer.NickName);
        roomOptions.CustomRoomProperties.Add("Betting", betting);
        roomOptions.CustomRoomProperties.Add("PlayerCount", 0);
        roomOptions.CustomRoomProperties.Add("MaxPlayer", maxCount);
        roomOptions.CustomRoomProperties.Add("State", "대기");
        roomOptions.CustomRoomPropertiesForLobby = new string[] {"RoomName", "Password", "MasterName", "Betting", "PlayerCount", "MaxPlayer", "State" };
        roomOptions.PublishUserId = true;

        if (PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default))
        {
            print("Create Room success");         
        }

    }

    public void OnClickJoinRoom(string roomName, bool isClose)
    {
        s = roomName;
        if (!isClose)
        {           
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            passwordChecker.SetActive(true);
            passwordChecker.GetComponent<Lobby_CheckPassword>().SetProperties(roomName);
        }                 
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print(message);
    }

    public override void OnJoinedRoom()
    {
        
        Lobby_ChatService.DisConnectChannel();
        LevelLoader.LoadNextLevel("Room");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print(message);
    }

   

}

